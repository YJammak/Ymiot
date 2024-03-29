﻿using Newtonsoft.Json;
using Ymiot.Core.Utils;

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
    public string Id { get; private set; } = default!;

    /// <summary>
    /// 房间名称
    /// </summary>
    [JsonProperty("name")]
    public string Name { get; private set; } = default!;

    [JsonProperty("bssid")]
    public string Bssid { get; private set; } = default!;

    /// <summary>
    /// 父名称
    /// </summary>
    [JsonProperty("parentid")]
    public string ParentId { get; private set; } = default!;

    /// <summary>
    /// 设备ID列表
    /// </summary>
    [JsonProperty("dids")]
    public IReadOnlyList<string>? Dids { get; private set; }

    [JsonProperty("shareflag")]
    public int ShareFlag { get; private set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    [JsonProperty("create_time")]
    [JsonConverter(typeof(UnixTimestampToDateTime))]
    public DateTime CreateTime { get; private set; }

    public override string ToString()
    {
        return $"{Name}({Dids?.Count ?? 0}) - {Id}";
    }
}
