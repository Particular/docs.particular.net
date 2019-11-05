namespace Extensions.Hosting_1
{
    using System.Threading.Tasks;
    using Microsoft.Extensions.Hosting;
    using NServiceBus;
    using NServiceBus.WebHost;

    class Configuration
    {
        async Task ConfigureHost()
        {
            #region extensions-host-configuration

            var host = Host.CreateDefaultBuilder()
                .UseNServiceBus(hostBuilderContext =>
                {
                    var endpointConfiguration = new EndpointConfiguration("MyConsoleEndpoint");
                    // configure endpoint here
                    return endpointConfiguration;
                })
                .Build();

            await host.RunAsync();

            #endregion
        }

        async Task ConfigureWebHost()
        {
            #region extensions-host-configuration-webhost
            var host = new HostBuilder()
                .ConfigureServices(serviceCollection =>
                {
                    var endpointConfiguration = new EndpointConfiguration("MyWebEndpoint");
                    // configure endpoint here

                    serviceCollection.AddNServiceBus(endpointConfiguration);
                })
                .Build();

            await host.RunAsync();
            #endregion
        }
    }
}
