using Newtonsoft.Json.Linq;

namespace Ymiot.Core.Utils;

public static class JObjectExt
{
    public static string GetString(this JObject obj, string propertyName)
    {
        var value = obj?.GetValue(propertyName);
        if (value is JObject jObject)
            return jObject.ToString();

        return value?.Value<string>();
    }

    public static int GetInt(this JObject obj, string propertyName, int defaultValue = -1)
    {
        return obj?.GetValue(propertyName)?.Value<int>() ?? defaultValue;
    }
}
