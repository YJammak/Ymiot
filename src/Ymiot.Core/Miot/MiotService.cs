using Newtonsoft.Json;

namespace Ymiot.Core.Miot;

public class MiotService : Miot
{
    [JsonIgnore]
    public int Siid => Iid;

    [JsonProperty("properties")]
    public IReadOnlyList<MiotProperty>? Properties { get; private set; }

    [JsonProperty("actions")]
    public IReadOnlyList<MiotAction>? Actions { get; private set; }
}
