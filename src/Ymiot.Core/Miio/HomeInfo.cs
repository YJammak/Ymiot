using Newtonsoft.Json;
using Ymiot.Core.Utils;

namespace Ymiot.Core.Miio;

/// <summary>
/// 家信息
/// </summary>
public class HomeInfo
{
    /// <summary>
    /// ID
    /// </summary>
    [JsonProperty("id")]
    public string Id { get; private set; }

    /// <summary>
    /// 名称
    /// </summary>
    [JsonProperty("name")]
    public string Name { get; private set; }

    [JsonProperty("bssid")]
    public string Bssid { get; private set; }

    /// <summary>
    /// 设备ID列表
    /// </summary>
    [JsonProperty("dids")]
    public IReadOnlyList<string> Dids { get; private set; }

    [JsonProperty("shareflag")]
    public int ShareFlag { get; private set; }

    [JsonProperty("permit_level")]
    public int PermitLevel { get; private set; }

    [JsonProperty("status")]
    public int Status { get; private set; }

    /// <summary>
    /// 经度
    /// </summary>
    [JsonProperty("longitude")]
    public double Longitude { get; private set; }

    /// <summary>
    /// 纬度
    /// </summary>
    [JsonProperty("latitude")]
    public double Latitude { get; private set; }

    /// <summary>
    /// 城市ID
    /// </summary>
    [JsonProperty("city_id")]
    public long CityId { get; private set; }

    /// <summary>
    /// 地址
    /// </summary>
    [JsonProperty("address")]
    public string Address { get; private set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    [JsonProperty("create_time")]
    [JsonConverter(typeof(UnixTimestampToDateTime))]
    public DateTime CreateTime { get; private set; }

    /// <summary>
    /// 房间列表
    /// </summary>
    [JsonProperty("roomlist")]
    public IReadOnlyList<RoomInfo> Rooms { get; private set; }


    [JsonProperty("uid")]
    public long Uid { get; private set; }

    public override string ToString()
    {
        return $"{Name}({Dids?.Count ?? 0}) - {Id}";
    }
}
