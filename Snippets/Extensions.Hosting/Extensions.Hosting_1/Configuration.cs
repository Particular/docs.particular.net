namespace Extensions.Hosting_1
{
    using System.Threading.Tasks;
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
    }
}
