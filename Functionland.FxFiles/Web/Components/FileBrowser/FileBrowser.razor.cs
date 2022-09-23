﻿using Functionland.FxFiles.App.Components.Common;
using Functionland.FxFiles.App.Components.DesignSystem;
using Functionland.FxFiles.App.Components.Modal;
using Functionland.FxFiles.Shared.Services.Contracts;

using Microsoft.Extensions.Localization;

namespace Functionland.FxFiles.App.Components;

public partial class FileBrowser
{
    private FsArtifact? _currentArtifact;

    private List<FsArtifact> _pins = new();

    private List<FsArtifact> _artifacts = new();

    private ArtifactSelectionModal? _artifactSelectionModalRef { get; set; }
    private ArtifactOverflowModal _asm { get; set; }

    [Parameter] public IPinService PinService { get; set; } = default!;

    [Parameter] public IFileService FileService { get; set; } = default!;

    protected override async Task OnInitAsync()
    {
        await LoadPinsAsync();

        await LoadChildrenArtifactsAsync();

        await base.OnInitAsync();
    }

    public async Task HandleCopyArtifactsAsync(List<FsArtifact> artifacts, CancellationToken cancellationToken)
    {
        List<FsArtifact> existArtifacts = new();
        string? destinationPath = await HandleSelectDestinationArtifact();
        if (string.IsNullOrWhiteSpace(destinationPath))
        {
            return;
        }

        try
        {
            await FileService.CopyArtifactsAsync(artifacts.ToArray(), destinationPath, false, cancellationToken);
        }
        catch (CanNotOperateOnFilesException ex)
        {
            existArtifacts = ex.FsArtifacts;
        }

        if (existArtifacts.Any())
        {
            //TODO: handle skip or replace artifactsToCopy
        }
    }

    public async Task HandleMoveArtifactsAsync(List<FsArtifact> artifacts, CancellationToken cancellationToken)
    {
        List<FsArtifact> existArtifacts = new();
        string? destinationPath = await HandleSelectDestinationArtifact();
        if (string.IsNullOrWhiteSpace(destinationPath))
        {
            return;
        }

        try
        {
            await FileService.CopyArtifactsAsync(artifacts.ToArray(), destinationPath, false, cancellationToken);
        }
        catch (CanNotOperateOnFilesException ex)
        {
            existArtifacts = ex.FsArtifacts;
        }

        if (existArtifacts.Any())
        {
            //TODO: handle skip or replace artifactsToMove
        }

    }

    public async Task<string?> HandleSelectDestinationArtifact()
    {
        var Result = await _artifactSelectionModalRef?.ShowAsync();
        string? destinationPath = null;

        if (Result?.ResultType == ArtifactSelectionResultType.Ok)
        {
            var destinationFsArtifact = Result.SelectedArtifacts.FirstOrDefault();
            destinationPath = destinationFsArtifact?.FullPath;
        }

        return destinationPath;
    }

    public async Task HandleRenameArtifact(FsArtifact artifact, string newName, CancellationToken cancellationToken)
    {
        string filePath = artifact.FullPath;
        try
        {
            await FileService.RenameFileAsync(filePath, newName, cancellationToken);
        }
        catch (DomainLogicException ex) when (ex.Message == Localizer.GetString(AppStrings.ArtifactPathIsNull, "file"))
        {
            //TODO: show exeception message with toast
        }
    }

    public void HandlePinArtifacts()
    {

    }

    public void HandleDeleteArtifacts()
    {

    }

    public void HandleShowDetailsArtifact()
    {

    }

    public void HandleCreateFolder()
    {

    }

    private async Task LoadPinsAsync()
    {
        var allPins = PinService.GetPinnedArtifactsAsync(null);

        var pins = new List<FsArtifact>();

        await foreach (var item in allPins)
        {
            pins.Add(item);
        }

        _pins = pins;
    }

    private async Task LoadChildrenArtifactsAsync(FsArtifact? parentArtifact = null)
    {
        var allFiles = FileService.GetArtifactsAsync(parentArtifact?.FullPath);

        var artifacts = new List<FsArtifact>();

        await foreach (var item in allFiles)
        {
            artifacts.Add(item);
        }

        _artifacts = artifacts;
    }

    private async Task HandleSelectArtifact(FsArtifact artifact)
    {
        _currentArtifact = artifact;
        await LoadChildrenArtifactsAsync(_currentArtifact);
        // load current artifacts
    }

    private async Task HandleOptionsArtifact(FsArtifact artifact)
    {
        var result = await _asm.ShowAsync();
        if (result.ResultType == ArtifactOverflowResultType.Delete)
        {
            Console.WriteLine($"Delete {artifact.Name}");
        }
    }

    private async Task HandleSelectedArtifactsOptions(List<FsArtifact> artifacts)
    {
        var isMultiple = artifacts.AsParallel().Count() > 1;

        var result = await _asm.ShowAsync(isMultiple);

        if (result.ResultType == ArtifactOverflowResultType.Delete)
        {
            foreach (var artifact in artifacts)
            {
                Console.WriteLine($"Delete {artifact.Name}");
            }
        }
    }

    private bool IsInRoot(FsArtifact? artifact)
    {
        return artifact is null ? true : false;
    }
}
