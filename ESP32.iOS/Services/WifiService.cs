using Foundation;
using NetworkExtension;
using UIKit;

namespace ESP32.Services;

internal class WifiService : IWifiService
{
    public Task<List<string>> ScanForWifiNetworks()
    {
        UIApplication.SharedApplication.OpenUrl(new NSUrl("App-Prefs:root=WIFI"));
        // iOS does not allow scanning for Wi-Fi networks.
        return Task.FromResult(new List<string>());
    }

    public Task<bool> ConnectToWifi(string ssid, string password)
    {
        var tcs = new TaskCompletionSource<bool>();

        var hotspotConfig = new NEHotspotConfiguration(ssid, password, false)
        {
            JoinOnce = true // Ensures connection is only attempted once
        };

        NEHotspotConfigurationManager.SharedManager.ApplyConfiguration(hotspotConfig, (NSError error) =>
        {
            if (error != null)
            {
                Console.WriteLine($"WiFi Connection Error: {error.LocalizedDescription}");
                tcs.SetResult(false);
            }
            else
            {
                Console.WriteLine("Successfully connected to Wi-Fi");
                tcs.SetResult(true);
            }
        });

        return tcs.Task;
    }
}
