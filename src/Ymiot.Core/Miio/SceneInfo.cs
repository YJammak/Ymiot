using Newtonsoft.Json;

namespace Ymiot.Core.Miio;

/// <summary>
/// 场景信息
/// </summary>
public class SceneInfo
{
    /// <summary>
    /// 场景ID
    /// </summary>
    [JsonProperty("scene_id")]
    public string Id { get; private set; }

    /// <summary>
    /// 用户ID
    /// </summary>
    [JsonProperty("uid")]
    public string Uid { get; private set; }

    /// <summary>
    /// 家ID
    /// </summary>
    [JsonProperty("home_id")]
    public string HomeId { get; private set; }

    /// <summary>
    /// 场景名称
    /// </summary>
    [JsonProperty("name")]
    public string Name { get; private set; }

    /// <summary>
    /// 模板ID
    /// </summary>
    [JsonProperty("template_id")]
    public string TemplateId { get; private set; }

    /// <summary>
    /// 类型
    /// </summary>
    [JsonProperty("type")]
    public int Type { get; private set; }

    /// <summary>
    /// 本地设备
    /// </summary>
    [JsonProperty("local_dev")]
    public string LocalDev { get; private set; }

    /// <summary>
    /// 是否激活
    /// </summary>
    [JsonProperty("enable")]
    public bool Enable { get; private set; }

    [JsonProperty("enable_push")]
    public bool EnablePush { get; private set; }

    [JsonProperty("common_use")]
    public bool CommonUse { get; private set; }

    [JsonProperty("timespan")]
    public string Timespan { get; private set; }

    public override string ToString()
    {
        return $"{Name}({Enable}) - {Id}";
    }
}
