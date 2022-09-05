using Newtonsoft.Json;

namespace Ymiot.Core.Miio;

/// <summary>
/// 设备信息
/// </summary>
public class DeviceInfo
{
    /// <summary>
    /// 设备名称
    /// </summary>
    [JsonProperty("name")]
    public string Name { get; private set; }

    /// <summary>
    /// 设备ID
    /// </summary>
    [JsonProperty("did")]
    public string Did { get; private set; }

    /// <summary>
    /// 设备型号
    /// </summary>
    [JsonProperty("model")]
    public string Model { get; private set; }

    /// <summary>
    /// 是否在线
    /// </summary>
    [JsonProperty("isOnline")]
    public bool IsOnline { get; private set; }

    /// <summary>
    /// 家庭编号
    /// </summary>
    [JsonProperty("family_id")]
    public long FamilyId { get; private set; }

    /// <summary>
    /// 父设备ID
    /// </summary>
    [JsonProperty("parent_id")]
    public string ParentId { get; private set; }

    /// <summary>
    /// 父设备型号
    /// </summary>
    [JsonProperty("parent_model")]
    public string ParentModel { get; private set; }

    public override string ToString()
    {
        return $"{Name} - {Did} - {Model}";
    }
}
