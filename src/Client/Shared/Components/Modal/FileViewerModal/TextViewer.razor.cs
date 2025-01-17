﻿using Microsoft.JSInterop;
using System.Reflection.Metadata;
using System.Text;

namespace Functionland.FxFiles.Client.Shared.Components.Modal;

public partial class TextViewer : IFileViewerComponent, IDisposable
{
    [Parameter] public IFileService FileService { get; set; } = default!;
    [Parameter] public FsArtifact? CurrentArtifact { get; set; }
    [Parameter] public EventCallback OnBack { get; set; }
    [Parameter] public EventCallback<List<FsArtifact>> OnPin { get; set; }
    [Parameter] public EventCallback<List<FsArtifact>> OnUnpin { get; set; }
    [Parameter] public EventCallback<FsArtifact> OnOptionClick { get; set; }

    [AutoInject] private ThemeInterop ThemeInterop = default!;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
           var _isSystemThemeDark = await ThemeInterop.GetThemeAsync() is FxTheme.Dark;
            await JSRuntime.InvokeVoidAsync("setupCodeMirror", _isSystemThemeDark);
            _ = GetTextAsync();
        }

        await base.OnAfterRenderAsync(firstRender);
    }

    private async Task HandlePinAsync()
    {
        if (CurrentArtifact is null) return;

        await OnPin.InvokeAsync(new List<FsArtifact>() { CurrentArtifact });
    }

    private async Task HandleUnpinAsync()
    {
        if (CurrentArtifact is null) return;

        await OnUnpin.InvokeAsync(new List<FsArtifact>() { CurrentArtifact });
    }

    private async Task HandleOptionClickAsync()
    {
        if (CurrentArtifact is null) return;

        await OnOptionClick.InvokeAsync(CurrentArtifact);
    }

    private async Task GetTextAsync()
    {
        if (CurrentArtifact?.FullPath == null) return;

        var text = File.ReadAllText(CurrentArtifact.FullPath, Encoding.UTF8);
        
        await JSRuntime.InvokeVoidAsync("setCodeMirrorText", text, CurrentArtifact.Name);
        await InvokeAsync(() => StateHasChanged());
    }

    public void Dispose()
    {
        JSRuntime.InvokeVoidAsync("unRegisterOnTouchEvent");
    }
}