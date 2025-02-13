using Android.Content;
using Android.Net;
using Android.Net.Wifi;
using Android.OS;

namespace ESP32.Services;

internal class WifiService : IWifiService
{
    private readonly WifiManager _wifiManager;

    public WifiService()
    {
        _wifiManager = (WifiManager)Android.App.Application.Context.GetSystemService(Context.WifiService)!;
    }

    public async Task<List<string>> ScanForWifiNetworks()
    {
        if (_wifiManager == null)
            return new List<string>();

        var scanSuccess = _wifiManager.StartScan();
        if (!scanSuccess)
            return new List<string>();

        await Task.Delay(3000); // Wait for scan results

        var results = _wifiManager.ScanResults;
        return results?.Select(r => r.Ssid).Distinct().ToList() ?? new List<string>();
    }

    public async Task<bool> ConnectToWifi(string ssid, string password)
    {
        if (_wifiManager == null)
            return false;

        if (Build.VERSION.SdkInt >= BuildVersionCodes.Q)
        {
            // Android 10+ requires using ConnectivityManager to suggest networks
            var wifiNetworkSpecifier = new WifiNetworkSpecifier.Builder()
                .SetSsid(ssid)
                .SetWpa2Passphrase(password)
                .Build();

            var networkRequest = new NetworkRequest.Builder()
                .AddTransportType(TransportType.Wifi)
                .SetNetworkSpecifier(wifiNetworkSpecifier)
                .Build();

            var connectivityManager = (ConnectivityManager)Android.App.Application.Context.GetSystemService(Context.ConnectivityService)!;
            var networkCallback = new WifiNetworkCallback();

            connectivityManager.RequestNetwork(networkRequest, networkCallback);

            return await networkCallback.ConnectedTask;
        }
        else
        {
            // For Android 9 and below, use WifiManager directly
            var wifiConfig = new WifiConfiguration
            {
                Ssid = $"\"{ssid}\"",
                PreSharedKey = $"\"{password}\""
            };

            int netId = _wifiManager.AddNetwork(wifiConfig);
            if (netId == -1) return false;

            _wifiManager.Disconnect();
            _wifiManager.EnableNetwork(netId, true);
            _wifiManager.Reconnect();
            return true;
        }
    }

    private class WifiNetworkCallback : ConnectivityManager.NetworkCallback
    {
        private readonly TaskCompletionSource<bool> _tcs = new();

        public Task<bool> ConnectedTask => _tcs.Task;

        public override void OnAvailable(Android.Net.Network network)
        {
            base.OnAvailable(network);
            _tcs.TrySetResult(true);
        }

        public override void OnUnavailable()
        {
            base.OnUnavailable();
            _tcs.TrySetResult(false);
        }
    }
}