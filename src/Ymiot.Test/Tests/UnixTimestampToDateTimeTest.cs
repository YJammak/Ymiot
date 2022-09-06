using FluentAssertions;
using Newtonsoft.Json;
using Ymiot.Core.Utils;

namespace Ymiot.Test.Tests;

public class UnixTimestampToDateTimeTest
{
    private class TestClass
    {
        [JsonProperty("time")]
        [JsonConverter(typeof(UnixTimestampToDateTime))]
        public DateTime DateTime { get; set; }
    }

    [Fact]
    public void ReadTest()
    {
        var dateTime = DateTime.Now;
        var timestamp = (dateTime - new DateTime(1970, 1, 1)).TotalSeconds;
        var json = $"{{\"time\": {timestamp:F0}}}";
        var test = JsonHelper.DeserializeToObject<TestClass>(json);
        test.Should().NotBeNull();
        test.DateTime.Should().BeCloseTo(dateTime, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void WriteTest()
    {
        var dateTime = DateTime.Now;
        var test = new TestClass
        {
            DateTime = dateTime
        };
        var json = JsonHelper.SerializeObject(test);
        json.Should().NotBeNull();
        json.Should().Contain(((long)(dateTime - DateTime.UnixEpoch).TotalSeconds).ToString());
    }
}
