using Newtonsoft.Json;

namespace Ymiot.Core.Miio;

/// <summary>
/// 设置设备属性参数
/// </summary>
public sealed class DeviceSetPropertyParam
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
    /// 属性ID(PIID)
    /// </summary>
    [JsonProperty("piid")]
    public int Piid { get; set; }

    /// <summary>
    /// 属性值
    /// </summary>
    [JsonProperty("value")]
    public object Value { get; set; } = default!;
}
