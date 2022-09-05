using Ymiot.Core.Api;

namespace Ymiot.Device;


public abstract class DeviceBase
{
    /// <summary>
    /// 设备型号名称
    /// </summary>
    public string ModelName { get; protected set; }

    /// <summary>
    /// 设备型号
    /// </summary>
    public string Model => Info?.Model;

    /// <summary>
    /// 设备名称
    /// </summary>
    public string Name => Info.Name;

    /// <summary>
    /// 设备信息
    /// </summary>
    public DeviceInfo Info { get; protected set; }

    /// <summary>
    /// 设备属性列表
    /// </summary>
    public IReadOnlyList<DeviceProperty> Properties { get; protected set; }

    protected DeviceBase(DeviceInfo info)
    {
        Info = info;
    }

    protected DeviceBase(string modelName, DeviceInfo info)
    {
        ModelName = modelName;
        Info = info;
    }
}
