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

    async Task ReadConnectionString()
    {
        #region extensions-host-connection-string

        var host = Host.CreateDefaultBuilder()
            .UseNServiceBus(ctx =>
            {
                var transportConnectionString = ctx.Configuration.GetConnectionString("Transport");

                var endpointConfiguration = new EndpointConfiguration("MyEndpoint");

                // Pass transportConnectionString to the transport, for example:
                //   var transport = endpointConfiguration.UseTransport<YourTransport>();
                //   transport.ConnectionString(transportConnectionString);
                _ = transportConnectionString;

                return endpointConfiguration;
            })
            .Build();

        await host.RunAsync();

        #endregion
    }

    #region extensions-host-options-pattern

    class EndpointSettings
    {
        public string EndpointName { get; set; } = "MyEndpoint";
        public int MaxConcurrency { get; set; } = 4;
    }

    async Task UseOptionsPattern()
    {
        var host = Host.CreateDefaultBuilder()
            .UseNServiceBus(ctx =>
            {
                var settings = ctx.Configuration
                    .GetSection("NServiceBus")
                    .Get<EndpointSettings>() ?? new EndpointSettings();

                var endpointConfiguration = new EndpointConfiguration(settings.EndpointName);
                endpointConfiguration.LimitMessageProcessingConcurrencyTo(settings.MaxConcurrency);

                return endpointConfiguration;
            })
            .Build();

        await host.RunAsync();
    }

    #endregion
}
