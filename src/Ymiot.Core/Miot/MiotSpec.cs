using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace Ymiot.Core.Miot;

public class MiotSpec : Miot
{
    [JsonProperty("services")]
    public IReadOnlyList<MiotService>? Services { get; private set; }

    public static async Task<MiotSpec?> FromModelAsync(string model, bool useRemote = false)
    {
        var type = await GetModelTypeAsync(model, useRemote);
        return await FromTypeAsync(type);
    }

    public static async Task<MiotSpec?> FromTypeAsync(string? type, CancellationToken token = default)
    {
        if (string.IsNullOrWhiteSpace(type))
            return null;

        // TODO 缓存
        var path = $"/miot-spec-v2/instance?type={type}";
        var jObject = await DownloadMiotSpec(path, 3, token: token);
        return jObject?.ToObject<MiotSpec>();
    }

    public static async Task<string?> GetModelTypeAsync(string model, bool useRemote = false, CancellationToken token = default)
    {
        if (string.IsNullOrWhiteSpace(model))
            return null;

        // TODO 缓存
        const string path = "/miot-spec-v2/instances?status=all";
        var jObject = await DownloadMiotSpec(path, 3, 60, token);
        var instances = jObject?.GetValue("instances");
        if (instances == null)
            return null;

        JToken? latest = null;
        foreach (var jToken in instances.ToArray())
        {
            var m = jToken.Value<string>("model");
            var status = jToken.Value<string>("status");
            if (status != "released" || m != model)
                continue;
            if (latest == null || latest.Value<int>("version") < jToken.Value<int>("version"))
                latest = jToken;
        }
        return latest?.Value<string>("type");
    }

    private static async Task<JObject?> DownloadMiotSpec(string path, int tries = 1, int timeout = 30, CancellationToken token = default)
    {
        var hosts = new[]
        {
            "https://miot-spec.org",
            "https://spec.miot-spec.com"
        };

        Exception? exception = null;
        while (tries > 0)
        {
            foreach (var host in hosts)
            {
                var url = $"{host}{path}";
                try
                {
                    return await DownloadMiotSpecImpl(url, timeout, token);
                }
                catch (Exception e)
                {
                    exception = e;
                }
            }

            tries--;
            await Task.Delay(1000, token);
        }

        if (exception != null)
            throw exception;

        return null;
    }

    private static async Task<JObject?> DownloadMiotSpecImpl(string url, int timeout = 30, CancellationToken token = default)
    {
        var client = new RestClient();
        var request = new RestRequest(url)
        {
            Timeout = timeout * 1000
        };
        var content = (await client.GetAsync(request, token)).Content;
        return string.IsNullOrWhiteSpace(content) ? null : JObject.Parse(content);
    }
}
