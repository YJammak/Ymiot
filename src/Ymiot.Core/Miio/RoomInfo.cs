using Newtonsoft.Json;

namespace Ymiot.Core.Miio;

/// <summary>
/// 房间信息
/// </summary>
public class RoomInfo
{
    /// <summary>
    /// 房间ID
    /// </summary>
    [JsonProperty("id")]
    public string Id { get; private set; }

    /// <summary>
    /// 房间名称
    /// </summary>
    [JsonProperty("name")]
    public string Name { get; private set; }

    /// <summary>
    /// 父名称
    /// </summary>
    [JsonProperty("parentid")]
    public string ParentId { get; private set; }

    /// <summary>
    /// 设备ID列表
    /// </summary>
    [JsonProperty("dids")]
    public IReadOnlyList<string> Dids { get; private set; }

    public override string ToString()
    {
        return $"{Name}({Dids?.Count ?? 0}) - {Id}";
    }
}
