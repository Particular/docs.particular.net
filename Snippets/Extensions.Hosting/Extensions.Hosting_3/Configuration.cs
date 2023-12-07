using System.Threading.Tasks;
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
}