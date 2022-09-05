using Newtonsoft.Json;
using Ymiot.Core.Utils;

namespace Ymiot.Core.Miot;

[JsonConverter(typeof(ArrayToMiotValueRange))]
public record MiotValueRange(double Min, double Max, double Step);
