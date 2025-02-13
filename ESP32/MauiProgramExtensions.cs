using ESP32.Services;
using Microsoft.Extensions.Logging;
using Plugin.BLE;
namespace ESP32
{
    public static class MauiProgramExtensions
    {
        public static MauiAppBuilder UseSharedMauiApp(this MauiAppBuilder builder)
        {
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddSingleton(CrossBluetoothLE.Current);
            builder.Services.AddSingleton(CrossBluetoothLE.Current.Adapter);
            builder.Services.AddSingleton<IBluetoothService, BluetoothService>();
            builder.Services.AddSingleton<MainPage>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder;
        }
    }
}
