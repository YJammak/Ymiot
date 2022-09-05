using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Ymiot.Test;

public class Startup
{
    public void ConfigureHost(IHostBuilder hostBuilder)
    {
        hostBuilder.ConfigureHostConfiguration(configurationBuilder =>
        {
            configurationBuilder.AddUserSecrets("e4139c79-8d3d-40ea-9aef-a0b9c2b656bc", true);
        });
    }

    public void ConfigureServices(IServiceCollection services)
    {

    }
}
