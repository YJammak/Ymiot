using Newtonsoft.Json;

namespace Ymiot.Core.Miot;

public class MiotAction : Miot
{
    [JsonIgnore]
    public int Aiid => Iid;

    [JsonProperty("in")]
    public IReadOnlyList<int>? Ins { get; private set; }

    [JsonProperty("out")]
    public IReadOnlyList<int>? Outs { get; private set; }
}
