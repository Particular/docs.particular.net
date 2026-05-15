using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using NServiceBus;

class Configuration
{
    async Task ConfigureHost()
    {
        #region extensions-host-configuration

        var hostBuilder = Host.CreateApplicationBuilder();

        var endpointConfiguration = new EndpointConfiguration("MyEndpoint");

        // configure endpoint here

        hostBuilder.UseNServiceBus(endpointConfiguration);

        var host = hostBuilder.Build();

        await host.RunAsync();

        #endregion
    }

    async Task ReadAppSettings()
    {
        #region extensions-host-appsettings

        var hostBuilder = Host.CreateApplicationBuilder();

        var endpointName = hostBuilder.Configuration.GetValue<string>("NServiceBus:EndpointName")
            ?? "MyEndpoint";

        var endpointConfiguration = new EndpointConfiguration(endpointName);
        // configure endpoint, passing values from hostBuilder.Configuration as needed

        hostBuilder.UseNServiceBus(endpointConfiguration);

        var host = hostBuilder.Build();

        await host.RunAsync();

        #endregion
    }
}