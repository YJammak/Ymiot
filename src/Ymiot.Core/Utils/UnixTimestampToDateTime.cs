using Newtonsoft.Json;

namespace Ymiot.Core.Utils;

public class UnixTimestampToDateTime : JsonConverter
{
    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        if (value is not DateTime dateTime)
            throw new FormatException("数据类型错误，无法完成转换");

        var timestamp = (dateTime - DateTime.UnixEpoch).TotalSeconds;
        writer.WriteValue((long)timestamp);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        if (objectType != typeof(DateTime) ||
            !long.TryParse(reader.Value?.ToString(), out var value))
            throw new FormatException("数据类型错误，无法完成转换");

        return new DateTime(1970, 1, 1).AddSeconds(value);
    }

    public override bool CanConvert(Type objectType)
    {
        return typeof(DateTime) == objectType;
    }
}
