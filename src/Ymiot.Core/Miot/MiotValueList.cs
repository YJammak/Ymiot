using Newtonsoft.Json;

namespace Ymiot.Core.Miot;

public class MiotValueList
{
    [JsonProperty("value")]
    public int Value { get; private set; }

    [JsonProperty("description")]
    public string Description { get; private set; } = default!;

    public override string ToString()
    {
        return $"{Value} - {Description}";
    }
}
