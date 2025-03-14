﻿using ESP32.Services;

namespace ESP32.Droid
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            builder
                .UseSharedMauiApp();
            builder.Services.AddSingleton<IWifiService, WifiService>();
            return builder.Build();
        }
    }
}
