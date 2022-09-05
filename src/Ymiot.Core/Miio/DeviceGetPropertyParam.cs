using Newtonsoft.Json;

namespace Ymiot.Core.Miio;

/// <summary>
/// 获取设备属性值参数
/// </summary>
public class DeviceGetPropertyParam
{
    /// <summary>
    /// 设备ID(DID)
    /// </summary>
    [JsonProperty("did")]
    public string Did { get; set; }

    /// <summary>
    /// 类型ID(SIID)
    /// </summary>
    [JsonProperty("siid")]
    public int Siid { get; set; }

    /// <summary>
    /// 属性ID(PIID)
    /// </summary>
    [JsonProperty("piid")]
    public int Piid { get; set; }
}
