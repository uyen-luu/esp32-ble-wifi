namespace ESP32.Services;

public interface IWifiService
{
    Task<List<string>> ScanForWifiNetworks();
    Task<bool> ConnectToWifi(string ssid, string password);
}