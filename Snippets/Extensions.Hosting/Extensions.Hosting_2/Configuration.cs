using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using NServiceBus;

class Configuration
{
    async Task ConfigureHost()
    {
        #region extensions-host-configuration

        var host = Host.CreateDefaultBuilder()
            .UseNServiceBus(hostBuilderContext =>
            {
                var endpointConfiguration = new EndpointConfiguration("MyEndpoint");
                // configure endpoint here
                return endpointConfiguration;
            })
            .Build();

        await host.RunAsync();

        #endregion
    }

    async Task ReadAppSettings()
    {
        #region extensions-host-appsettings

        var host = Host.CreateDefaultBuilder()
            .UseNServiceBus(ctx =>
            {
                var endpointName = ctx.Configuration.GetValue<string>("NServiceBus:EndpointName")
                    ?? "MyEndpoint";

                var endpointConfiguration = new EndpointConfiguration(endpointName);
                // configure endpoint, passing values from ctx.Configuration as needed
                return endpointConfiguration;
            })
            .Build();

        await host.RunAsync();

        #endregion
    }
}