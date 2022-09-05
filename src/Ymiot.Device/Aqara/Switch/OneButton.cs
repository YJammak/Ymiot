using Ymiot.Core.Api;

namespace Ymiot.Device.Aqara.Switch;

public class OneButton : DeviceBase
{
    public OneButton(string modelName, DeviceInfo info) : base(modelName, info)
    {
        Properties = new[]
        {
            new DeviceProperty(
                "开关",
                2,
                1,
                ValueType.Bool,
                null,
                new Dictionary<object, string>
                {
                    {true, "开"},
                    {false, "关"},
                },
                Permission.Read | Permission.Write | Permission.Notice),
            new DeviceProperty(
                "模式",
                2,
                2,
                ValueType.Unit8,
                null,
                new Dictionary<object, string>
                {
                    {0, "有线和无线"},
                    {1, "无线"},
                },
                Permission.Read | Permission.Write | Permission.Notice)
        };
    }
}
