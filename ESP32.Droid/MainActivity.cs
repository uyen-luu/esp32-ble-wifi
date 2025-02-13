using Android;
using Android.App;
using Android.Content.PM;
using Android.OS;

namespace ESP32.Droid;

[Activity(Label = "ESP32", Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
    protected override void OnCreate(Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
        RequestPermissions();
    }

    private void RequestPermissions()
    {
        string[] permissions =
        [
            Manifest.Permission.BluetoothScan,
            Manifest.Permission.BluetoothConnect,
            Manifest.Permission.NearbyWifiDevices,
            Manifest.Permission.AccessFineLocation
        ];

        RequestPermissions(permissions, 0);
    }
}
