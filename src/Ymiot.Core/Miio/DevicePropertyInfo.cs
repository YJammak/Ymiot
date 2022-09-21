using Newtonsoft.Json;
using Ymiot.Core.Utils;

namespace Ymiot.Core.Miio;

/// <summary>
/// 设备属性值
/// </summary>
public class DevicePropertyInfo
{
    /// <summary>
    /// 设备ID(DID)
    /// </summary>
    [JsonProperty("did")]
    public string Did { get; private set; } = default!;

    /// <summary>
    /// 类型ID(SIID)
    /// </summary>
    [JsonProperty("siid")]
    public int Siid { get; private set; }

    /// <summary>
    /// 属性ID(PIID)
    /// </summary>
    [JsonProperty("piid")]
    public int Piid { get; private set; }

    /// <summary>
    /// 属性值
    /// </summary>
    [JsonProperty("value")]
    public object Value { get; private set; } = default!;

    /// <summary>
    /// 更新时间
    /// </summary>
    [JsonProperty("updateTime")]
    [JsonConverter(typeof(UnixTimestampToDateTime))]
    public DateTime UpdateTime { get; private set; }

    /// <summary>
    /// 执行用时
    /// </summary>
    [JsonProperty("exe_time")]
    public int ExecuteTime { get; private set; }

    public override string ToString()
    {
        return $"{Did} - {Siid} - {Piid} - {Value}";
    }
}
