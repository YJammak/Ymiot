using System.Collections.Concurrent;
using Ymiot.Core.Api;
using Ymiot.Device.Aqara.Switch;

namespace Ymiot.Device;

public class DeviceManager
{
    private static DeviceManager _instance;

    public static DeviceManager Instance => _instance ??= new DeviceManager();

    private readonly ConcurrentDictionary<string, KeyValuePair<string, Type>> _devices = new();

    public void Register(string model, string modelName, Type deviceType)
    {
        if (!deviceType.IsSubclassOf(typeof(DeviceBase)))
            throw new AggregateException("deviceType必须继承DeviceBase类");

        if (!_devices.TryAdd(model, new KeyValuePair<string, Type>(modelName, deviceType)))
            throw new AggregateException("该设备型号已存在");
    }

    public bool Unregister(string model)
    {
        return _devices.Remove(model, out _);
    }

    public DeviceBase DeviceFor(DeviceInfo device)
    {
        if (device == null)
            throw new ArgumentNullException(nameof(device));

        if (string.IsNullOrEmpty(device.Model))
            throw new ArgumentException("没有设备型号信息");

        if (!_devices.TryGetValue(device.Model, out var deviceInfo))
            throw new Exception($"没有找到型号为{device.Model}的设备");

        return Activator.CreateInstance(deviceInfo.Value, deviceInfo.Key, device) as DeviceBase;
    }

    private DeviceManager()
    {
        Register("lumi.ctrl_neutral1.v1", "Aqara 墙壁开关(单火线单键版)", typeof(OneButton));
    }
}
