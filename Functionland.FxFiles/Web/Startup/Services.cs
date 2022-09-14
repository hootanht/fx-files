﻿#if BlazorServer
using System.IO.Compression;
using Functionland.FxFiles.Shared.Services;
using Functionland.FxFiles.Shared.TestInfra.Contracts;
using Functionland.FxFiles.Shared.TestInfra.Implementations;

using Microsoft.AspNetCore.ResponseCompression;

namespace Functionland.FxFiles.App.Startup;

public static class Services
{
    public static void Add(IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpContextAccessor();
        services.AddRazorPages();
        services.AddServerSideBlazor();
        services.AddResponseCompression(opts =>
        {
            opts.EnableForHttps = true;
            opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[] { "application/octet-stream" }).ToArray();
            opts.Providers.Add<BrotliCompressionProvider>();
            opts.Providers.Add<GzipCompressionProvider>();
        })
            .Configure<BrotliCompressionProviderOptions>(opt => opt.Level = CompressionLevel.Fastest)
            .Configure<GzipCompressionProviderOptions>(opt => opt.Level = CompressionLevel.Fastest);
        services.AddAppServices();

        var fakeFileService = FakeFileServiceFactory.CreateTypical();

        services.AddSingleton<IFileService>((_) => fakeFileService);
        services.AddSingleton<IPlatformTestService, FakePlatformTestService>();
        services.AddTransient<FakeFileServicePlatformTest>();
    }
}
#endif
