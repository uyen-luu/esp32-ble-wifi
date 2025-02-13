using Plugin.BLE.Abstractions.Contracts;

namespace ESP32.Services;

public interface IBluetoothService
{
    Task ScanAndConnect(string deviceName);
}
internal class BluetoothService(IAdapter adapter) : IBluetoothService
{
    private IDevice? _device;


    public async Task ScanAndConnect(string deviceName)
    {
        adapter.DeviceDiscovered += (s, a) =>
        {
            if (a.Device.Name == deviceName)
            {
                _device = a.Device;
            }
        };

        await adapter.StartScanningForDevicesAsync();
        var a = adapter.BondedDevices.ToList();

        if (_device != null)
        {
            await adapter.ConnectToDeviceAsync(_device);
        }
    }
}