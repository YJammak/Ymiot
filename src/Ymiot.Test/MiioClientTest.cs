﻿using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Ymiot.Core.Miio;
using Ymiot.Core.Miot;

namespace Ymiot.Test;

[TestCaseOrderer("Ymiot.Test.PriorityOrderer", "Ymiot.Test")]
public class MiioClientTest
{
    private static MiioClient MiioClient { get; set; }
    private static IReadOnlyList<DeviceInfo> Devices { get; set; }
    private static HomeInfo Home { get; set; }

    private IConfiguration Configuration { get; set; }

    public MiioClientTest(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    [Fact]
    [TestPriority(0)]
    public async void LoginTest()
    {
        var sid = Configuration["UserInfo:Sid"];
        var username = Configuration["UserInfo:UserName"];
        var password = Configuration["UserInfo:Password"];
        var miioClient = new MiioClient();
        var logInfo = await miioClient.LoginAsync(sid, username, password);

        logInfo.Should().NotBeNull();
        logInfo.IsSuccessful.Should().BeTrue();
        logInfo.Sid.Should().NotBeNullOrEmpty();
        logInfo.UserId.Should().NotBeNullOrEmpty();
        logInfo.DeviceId.Should().NotBeNullOrEmpty();
        logInfo.ServiceToken.Should().NotBeNullOrEmpty();
        logInfo.SecurityToken.Should().NotBeNullOrEmpty();

        MiioClient = miioClient;
    }

    [Fact]
    [TestPriority(1)]
    public async void DeviceListTest()
    {
        var devices = await MiioClient.GetAllDevicesAsync();
        devices.Should().NotBeNullOrEmpty();
        Devices = devices;

        var d = devices.First();
        var spec = await MiotSpec.FromModelAsync(d.Model);
        spec.Should().NotBeNull();
        var service = spec.Services.FirstOrDefault(s => s.Siid == 1);
        service.Should().NotBeNull();
        var properties = service!.Properties.Select(p => new DeviceGetPropertyParam
        {
            Did = d.Did,
            Siid = service.Siid,
            Piid = p.Piid
        }).ToArray();
        var values = await MiioClient.GetDevicePropertyAsync(properties);
        values.Should().NotBeEmpty();
    }

    [Fact]
    [TestPriority(2)]
    public async void GetDevicePropertyTest()
    {
        var device = Devices.FirstOrDefault(d => d.Name == "走廊灯");
        device.Should().NotBeNull();
        var propertyParam = new DeviceGetPropertyParam
        {
            Did = device!.Did,
            Piid = 1,
            Siid = 2
        };
        var property = await MiioClient.GetDevicePropertyAsync(new[] { propertyParam });
        property.Should().NotBeNull();
    }

    [Fact]
    [TestPriority(3)]
    public async Task SetDevicePropertyTest()
    {
        var device = Devices.FirstOrDefault(d => d.Name == "走廊灯");
        device.Should().NotBeNull();
        var propertyParam = new DeviceSetPropertyParam
        {
            Did = device!.Did,
            Piid = 1,
            Siid = 2,
            Value = true
        };
        var result = await MiioClient.SetDevicePropertyAsync(new[] { propertyParam });
        result.Should().NotBeNull();
        result.Should().HaveCount(1);
        result[0].Code.Should().Be(0);

        await Task.Delay(1000);

        propertyParam.Value = false;
        result = await MiioClient.SetDevicePropertyAsync(new[] { propertyParam });
        result.Should().NotBeNull();
        result.Should().HaveCount(1);
        result[0].Code.Should().Be(0);
    }

    [Fact]
    [TestPriority(4)]
    public async void RoomListTest()
    {
        var deviceList = await MiioClient.GetAllHomesAsync();
        deviceList.Should().NotBeNullOrEmpty();
        deviceList[0].Rooms.Should().NotBeNullOrEmpty();
        Home = deviceList[0];
    }

    [Fact]
    [TestPriority(5)]
    public async void SceneListTest()
    {
        var scenes = await MiioClient.GetAllScenesAsync(Home.Id);
        scenes.Should().NotBeNullOrEmpty();
    }
}
