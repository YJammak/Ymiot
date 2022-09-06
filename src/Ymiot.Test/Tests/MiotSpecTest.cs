using FluentAssertions;
using Ymiot.Core.Miot;
using Ymiot.Test.Utils;

namespace Ymiot.Test.Tests;

[TestCaseOrderer("Ymiot.Test.PriorityOrderer", "Ymiot.Test")]
public class MiotSpecTest
{
    [Fact]
    [TestPriority(0)]
    public async void GetModelTypeTest()
    {
        var type = await MiotSpec.GetModelTypeAsync("lumi.ctrl_neutral1.v1");
        type.Should().NotBeNullOrWhiteSpace();
        type.Should().StartWith("urn:miot-spec-v2:device:switch:0000A003:lumi-ctrl-neutral1-v1");
    }

    [Fact]
    [TestPriority(1)]
    public async void FromTypeTest()
    {
        var miotSpec = await MiotSpec.FromTypeAsync("urn:miot-spec-v2:device:air-conditioner:0000A004:lumi-acn05:1");
        miotSpec.Should().NotBeNull();
        miotSpec.Services.Should().NotBeEmpty();
    }
}
