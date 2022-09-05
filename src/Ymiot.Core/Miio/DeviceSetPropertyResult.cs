using Newtonsoft.Json;

namespace Ymiot.Core.Miio;

/// <summary>
/// 设置属性值结果
/// </summary>
public class DeviceSetPropertyResult
{
    /// <summary>
    /// 设备ID(DID)
    /// </summary>
    [JsonProperty("did")]
    public string Did { get; private set; }

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
    /// 结果代码(0为成功)
    /// </summary>
    [JsonProperty("code")]
    public int Code { get; private set; }

    /// <summary>
    /// 执行用时
    /// </summary>
    [JsonProperty("exe_time")]
    public int ExecuteTime { get; private set; }

    public override string ToString()
    {
        return $"{Did} - {Siid} - {Piid} - {Code}";
    }
}
