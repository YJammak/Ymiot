using Ymiot.Core.Api;

namespace Ymiot.Device;

public static class MiioClientExt
{
    public static DevicePropertyInfo GetDeviceProperty(
        this MiioClient client,
        DeviceBase device,
        DeviceProperty property)
    {
        return client.GetDeviceProperty(new[]
        {
            new DeviceGetPropertyParam
            {
                DeviceId = device.Info.DeviceId,
                SymbolId = property.SymbolId,
                PropertyId = property.PropertyId
            }
        }).FirstOrDefault();
    }

    public static IReadOnlyList<DevicePropertyInfo> GetDeviceAllProperty(
        this MiioClient client,
        DeviceBase device)
    {
        return client.GetDeviceProperty(device.Properties.Select(p => new DeviceGetPropertyParam
        {
            DeviceId = device.Info.DeviceId,
            SymbolId = p.SymbolId,
            PropertyId = p.PropertyId
        }).ToList());
    }


}
