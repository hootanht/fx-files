﻿using Functionland.FxFiles.Client.Shared.Enums;
using Functionland.FxFiles.Client.Shared.Models;
using Functionland.FxFiles.Client.Shared.Services.Contracts;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Prism.Events;
using System.Text;

namespace Functionland.FxFiles.Client.Test.UnitTests
{
    [TestClass]
    public class PinServiceUnitTest : TestBase
    {
        [TestMethod]
        public async Task AddPinUnitTest_MustWork()
        {
            var testHost = Host.CreateDefaultBuilder()
               .ConfigureServices((_, services) =>
               {
                   services.AddClientSharedServices();
                   services.AddClientTestServices();
               }
            ).Build();

            var serviceScope = testHost.Services.CreateScope();
            var serviceProvider = serviceScope.ServiceProvider;

            var pinService = serviceProvider.GetService<IPinService>();
            var localdbService = serviceProvider.GetService<IFxLocalDbService>();
            var fileService = serviceProvider.GetService<IFileService>();
            await fileService.CreateFileAsync("E:\\Pic\\20170112_134108.jpg", GetSampleFileStream());
            await localdbService.InitAsync();
            await pinService.InitializeAsync();

            var artifact = new FsArtifact("E:\\Pic\\20170112_134108.jpg", "20170112_134108.jpg", FsArtifactType.Folder, FsFileProviderType.InternalMemory) { FileExtension = ".jpg" };
            await pinService.SetArtifactsPinAsync(
              new FsArtifact[] { artifact });

            var pinnedFiles = await pinService.GetPinnedArtifactsAsync();
            foreach (var file in pinnedFiles)
            {
                Console.WriteLine(file.FullPath);
            }
            await pinService.SetArtifactsUnPinAsync(new string[] { "C:\\Program Files" });
            pinnedFiles = await pinService.GetPinnedArtifactsAsync();
            foreach (var file in pinnedFiles)
            {
                Console.WriteLine(file.FullPath);
            }

        }
        private Stream GetSampleFileStream()
        {
            var sampleText = "Hello streamer!";
            byte[] byteArray = Encoding.ASCII.GetBytes(sampleText);
            MemoryStream stream = new MemoryStream(byteArray);
            return stream;
        }
        [TestMethod]
        public async Task PublishEvent_MustWork()
        {
            var testHost = Host.CreateDefaultBuilder()
               .ConfigureServices((_, services) =>
               {
                   services.AddClientSharedServices();
                   services.AddClientTestServices();
               }
            ).Build();

            var serviceScope = testHost.Services.CreateScope();
            var serviceProvider = serviceScope.ServiceProvider;

            var pinService = serviceProvider.GetService<IPinService>();
            var aggrigator = serviceProvider.GetService<IEventAggregator>();
            var localdbService = serviceProvider.GetService<IFxLocalDbService>();

            await localdbService.InitAsync();
            await pinService.InitializeAsync();

            await pinService.SetArtifactsPinAsync(
            new FsArtifact[] { new FsArtifact("E:\\Pic\\20170112_134108.jpg", "20170112_134108.jpg", FsArtifactType.File, FsFileProviderType.InternalMemory) { FileExtension = ".jpg" } });



            aggrigator.GetEvent<ArtifactChangeEvent>().Publish(new ArtifactChangeEvent()
            {
                ChangeType = FsArtifactChangesType.Modify,
                FsArtifact = new FsArtifact("E:\\Pic\\20170112_134108.jpg", "20170112_134108.jpg", FsArtifactType.Folder, FsFileProviderType.InternalMemory)

            });
        }

        [TestMethod]
        public async Task GetPins_MustWork()
        {
            var testHost = Host.CreateDefaultBuilder()
               .ConfigureServices((_, services) =>
               {
                   services.AddClientSharedServices();
                   services.AddClientTestServices();

               }
            ).Build();

            var serviceScope = testHost.Services.CreateScope();
            var serviceProvider = serviceScope.ServiceProvider;

            var pinService = serviceProvider.GetService<IPinService>();
            var aggrigator = serviceProvider.GetService<IEventAggregator>();
            var localdbService = serviceProvider.GetService<IFxLocalDbService>();

            await localdbService.InitAsync();
            await pinService.InitializeAsync();

            var artifact1 = new FsArtifact("E:\\Pic\\", "Pic", FsArtifactType.Folder, FsFileProviderType.InternalMemory);
            await pinService.SetArtifactsPinAsync(new FsArtifact[] { artifact1 });

            await pinService.SetArtifactsPinAsync(
                new FsArtifact[] { new FsArtifact("E:\\Pic\\MyPic\\", "MyPic", FsArtifactType.Folder, FsFileProviderType.InternalMemory) });

            var allPinnedFile = await pinService.GetPinnedArtifactsAsync();

            foreach (var file in allPinnedFile)
            {
                Console.WriteLine(file.FullPath);
            }
            var pathPinnedFiles = pinService.IsPinned(artifact1);

        }

    }
}
