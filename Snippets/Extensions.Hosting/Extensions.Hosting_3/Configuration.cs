using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using NServiceBus;

class Configuration
{
    async Task ConfigureHost()
    {
        #region extensions-host-configuration

        var hostBuilder = Host.CreateApplicationBuilder();

        hostBuilder.UseNServiceBus(() =>
        {
            var endpointConfiguration = new EndpointConfiguration("MyEndpoint");
            // configure endpoint here
            return endpointConfiguration;
        });

        var host = hostBuilder.Build();
        
        await host.RunAsync();

        #endregion
    }
}