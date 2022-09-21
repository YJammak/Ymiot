using Newtonsoft.Json;

namespace Ymiot.Core.Miot;

public class MiotProperty : Miot
{
    [JsonIgnore]
    public int Piid => Iid;

    [JsonProperty("format")]
    public string Format { get; private set; } = default!;

    [JsonProperty("access")]
    public IReadOnlyList<string> Accesses { get; private set; } = default!;

    [JsonProperty("unit")]
    public string Unit { get; private set; } = default!;

    [JsonProperty("value-list")]
    public IReadOnlyList<MiotValueList>? ValueList { get; private set; }

    [JsonProperty("value-range")]
    public MiotValueRange? ValueRange { get; private set; }

    [JsonIgnore]
    public bool Readable => Accesses?.Contains("read") ?? false;

    [JsonIgnore]
    public bool Writable => Accesses?.Contains("write") ?? false;

    [JsonIgnore]
    public bool Notifiable => Accesses?.Contains("notify") ?? false;
}
