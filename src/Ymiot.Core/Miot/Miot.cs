using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace Ymiot.Core.Miot;

public class Miot
{
    public static readonly IReadOnlyDictionary<string, string> SpecErrors = new Dictionary<string, string>()
    {
        {"000", "Unknown"},
        {"001", "Device does not exist"},
        {"002", "Service does not exist"},
        {"003", "Property does not exist"},
        {"004", "Event does not exist"},
        {"005", "Action does not exist"},
        {"006", "Device description not found"},
        {"007", "Device cloud not found"},
        {"008", "Invalid IID (PID, SID, AID, etc.)"},
        {"009", "Scene does not exist"},
        {"011", "Device offline"},
        {"013", "Property is not readable"},
        {"023", "Property is not writable"},
        {"033", "Property cannot be subscribed"},
        {"043", "Property value error"},
        {"034", "Action return value error"},
        {"015", "Action execution error"},
        {"025", "The number of action parameters does not match"},
        {"035", "Action parameter error"},
        {"036", "Device operation timeout"},
        {"100", "The device cannot be operated in its current state"},
        {"101", "IR device does not support this operation"},
        {"901", "Token does not exist or expires"},
        {"902", "Token is invalid"},
        {"903", "Authorization expired"},
        {"904", "Unauthorized voice device"},
        {"905", "Device not bound"},
        {"999", "Feature not online"},
        {"-4001", "Property is not readable"},
        {"-4002", "Property is not writable"},
        {"-4003", "Property/Action/Event does not exist"},
        {"-4004", "Other internal errors"},
        {"-4005", "Property value error"},
        {"-4006", "Action in parameters error"},
        {"-4007", "did error"}
    };

    [JsonProperty("iid")]
    public int Iid { get; private set; }

    [JsonProperty("type")]
    public string Type { get; private set; }

    [JsonProperty("description")]
    public string Description { get; private set; }

    [JsonIgnore]
    public string Name => FormatName(Type);

    public override string ToString()
    {
        return $"{Iid} - {Description} - {Type}";
    }

    public static string FormatName(string name)
    {
        if (string.IsNullOrEmpty(name))
            return null;

        name = name.Trim();
        return Regex.Replace(name, @"\W+", "_").ToLower();
    }

    public static string FormatDescName(string des, string name)
    {
        if (string.IsNullOrEmpty(des) || Regex.IsMatch(des, "[^x00-xff]"))
            return FormatName(name);

        return FormatName(des);
    }

    public static string NameByType(string type)
    {
        var array = $"{type}:::".Split(':');
        var name = array[3];
        return FormatName(name);
    }

    public static string SpecError(long errorNumber)
    {
        var errorInfo = errorNumber.ToString();
        var code = errorInfo;
        if (errorInfo.StartsWith("-70"))
            code = errorInfo[^3..];

        if (SpecErrors.TryGetValue(code, out var error))
            errorInfo += $" {error}";

        return errorInfo;
    }
}
