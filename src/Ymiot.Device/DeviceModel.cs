namespace Ymiot.Device;

public class DeviceModel
{
    public string Model { get; }

    public string Name { get; }

    public IReadOnlyList<DeviceProperty> Properties { get; }
}
