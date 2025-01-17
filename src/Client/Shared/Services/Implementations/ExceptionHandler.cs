﻿using System.Diagnostics;

using Microsoft.AppCenter.Crashes;
namespace Functionland.FxFiles.Client.Shared.Services.Implementations;

public partial class ExceptionHandler : IExceptionHandler
{
    [AutoInject] IStringLocalizer<AppStrings> _localizer = default!;

    public void Handle(Exception exception, IDictionary<string, object?>? parameters = null)
    {
#if DEBUG
        var title = _localizer.GetString(AppStrings.ToastErrorTitle);
        var message = (exception as KnownException)?.Message ?? exception.ToString(); ;
        FxToast.Show(title, message, FxToastType.Error);
        Console.WriteLine(message);
        Debugger.Break();
#else
        if (exception is KnownException knownException)
        {
            var title = _localizer.GetString(AppStrings.ToastErrorTitle);
            var message = exception.Message;
            FxToast.Show(title, message, FxToastType.Error);
        }
        else
        {
            if (DeviceInfo.Current.Platform != DevicePlatform.macOS && DeviceInfo.Current.Platform != DevicePlatform.MacCatalyst)
            {
                Crashes.TrackError(exception);
            }

            var title = _localizer.GetString(AppStrings.ToastErrorTitle);
            var message = _localizer.GetString(AppStrings.TheOpreationFailedMessage);
            FxToast.Show(title, message, FxToastType.Error);
        }
#endif

    }

    public void Track(Exception exception, IDictionary<string, object?>? parameters = null)
    {
#if DEBUG
        var message = (exception as KnownException)?.Message ?? exception.ToString();
        Console.WriteLine(message);
        Debugger.Break();
#else
        Crashes.TrackError(exception);
#endif
    }
}
