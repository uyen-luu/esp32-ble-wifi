using ESP32.Services;

namespace ESP32;

public partial class MainPage : ContentPage
{
    private readonly IBluetoothService _bluetoothService;
    private readonly IWifiService _wifiService;

    // ✅ Parameterless constructor for MAUI (Required)
    public MainPage() : this(null!, null!) { }

    // ✅ Constructor with Dependency Injection
    public MainPage(IBluetoothService bluetoothService, IWifiService wifiService)
    {
        InitializeComponent();
        _bluetoothService = bluetoothService ?? throw new ArgumentNullException(nameof(bluetoothService));
        _wifiService = wifiService ?? throw new ArgumentNullException(nameof(wifiService));
    }
    private async void OnScanBluetoothClicked(object sender, EventArgs e)
    {
        await _bluetoothService.ScanAndConnect("ESP32");
    }

    private async void OnScanWifiClicked(object sender, EventArgs e)
    {
        List<string> networks = await _wifiService.ScanForWifiNetworks();
        string result = networks.Count > 0 ? string.Join("\n", networks) : "No networks found.";
        await DisplayAlert("Available Wi-Fi Networks", result, "OK");
    }

    private async void OnConnectWifiClicked(object sender, EventArgs e)
    {
        if (!string.IsNullOrWhiteSpace(wifiSsid.Text) && !string.IsNullOrWhiteSpace(wifiPassword.Text))
        {
            bool success = await _wifiService.ConnectToWifi(wifiSsid.Text, wifiPassword.Text);
            await DisplayAlert("Wi-Fi Connection", success ? "Connected Successfully" : "Failed to Connect", "OK");
        }
    }
}
