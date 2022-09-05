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

    [JsonProperty("token")]
    public string Token { get; private set; }

    [JsonProperty("longitude")]
    public string Longitude { get; private set; }

    [JsonProperty("latitude")]
    public string Latitude { get; private set; }

    [JsonProperty("pid")]
    public string Pid { get; private set; }

    [JsonProperty("localip")]
    public string LocalIp { get; private set; }

    [JsonProperty("mac")]
    public string Mac { get; private set; }

    [JsonProperty("ssid")]
    public string Ssid { get; private set; }

    [JsonProperty("bssid")]
    public string Bssid { get; private set; }

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

    [JsonProperty("show_mode")]
    public int ShowMode { get; private set; }

    /// <summary>
    /// 设备型号
    /// </summary>
    [JsonProperty("model")]
    public string Model { get; private set; }

    [JsonProperty("adminFlag")]
    public int AdminFlag { get; private set; }

    [JsonProperty("shareFlag")]
    public int ShareFlag { get; private set; }

    [JsonProperty("permitLevel")]
    public int PermitLevel { get; private set; }

    /// <summary>
    /// 是否在线
    /// </summary>
    [JsonProperty("isOnline")]
    public bool IsOnline { get; private set; }

    [JsonProperty("desc")]
    public string Desc { get; private set; }

    [JsonProperty("extra")]
    public IReadOnlyDictionary<string, object> Extra { get; private set; }

    [JsonProperty("uid")]
    public long Uid { get; private set; }

    [JsonProperty("pd_id")]
    public long PdId { get; private set; }

    [JsonProperty("password")]
    public string Password { get; private set; }

    [JsonProperty("p2p_id")]
    public string P2PId { get; private set; }

    [JsonProperty("rssi")]
    public int Rssi { get; private set; }

    /// <summary>
    /// 家庭编号
    /// </summary>
    [JsonProperty("family_id")]
    public long FamilyId { get; private set; }

    [JsonProperty("reset_flag")]
    public int ResetFlag { get; private set; }

    public override string ToString()
    {
        return $"{Name} - {Did} - {Model}";
    }
}
