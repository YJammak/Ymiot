using Newtonsoft.Json;
using Ymiot.Core.Miot;

namespace Ymiot.Core.Utils;

public class ArrayToMiotValueRange : JsonConverter
{
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        if (value is not MiotValueRange valueRange)
            throw new FormatException("数据类型错误，无法完成转换");

        writer.WriteValue(new[]
        {
            valueRange.Min,
            valueRange.Max,
            valueRange.Step
        });
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        if (objectType != typeof(MiotValueRange) || reader.TokenType != JsonToken.StartArray)
            throw new FormatException("数据类型错误，无法完成转换");

        var min = reader.ReadAsDouble();
        var max = reader.ReadAsDouble();
        var step = reader.ReadAsDouble();

        while (reader.TokenType != JsonToken.EndArray)
        {
            reader.Read();
        }

        if (min == null || max == null || step == null)
            throw new FormatException("数据类型错误，无法完成转换");

        return new MiotValueRange(min.Value, max.Value, step.Value);
    }

    public override bool CanConvert(Type objectType)
    {
        return typeof(MiotValueRange) == objectType;
    }
}
