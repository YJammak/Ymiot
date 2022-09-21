using Newtonsoft.Json;

namespace Ymiot.Core.Miio;

/// <summary>
/// 设备动作参数
/// </summary>
public sealed class DeviceActionParam
{
    /// <summary>
    /// 设备ID(DID)
    /// </summary>
    [JsonProperty("did")]
    public string Did { get; set; } = default!;

    /// <summary>
    /// 类型ID(SIID)
    /// </summary>
    [JsonProperty("siid")]
    public int Siid { get; set; }

    /// <summary>
    /// 动作ID(AIID)
    /// </summary>
    [JsonProperty("aiid")]
    public int Aiid { get; set; }
}
