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

    async Task ReadConnectionString()
    {
        #region extensions-host-connection-string

        var hostBuilder = Host.CreateApplicationBuilder();

        var transportConnectionString = hostBuilder.Configuration.GetConnectionString("Transport");

        var endpointConfiguration = new EndpointConfiguration("MyEndpoint");

        // Pass transportConnectionString to the transport, for example:
        //   var transport = endpointConfiguration.UseTransport<YourTransport>();
        //   transport.ConnectionString(transportConnectionString);
        _ = transportConnectionString;

        hostBuilder.UseNServiceBus(endpointConfiguration);

        var host = hostBuilder.Build();

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
        var hostBuilder = Host.CreateApplicationBuilder();

        var settings = hostBuilder.Configuration
            .GetSection("NServiceBus")
            .Get<EndpointSettings>() ?? new EndpointSettings();

        var endpointConfiguration = new EndpointConfiguration(settings.EndpointName);
        endpointConfiguration.LimitMessageProcessingConcurrencyTo(settings.MaxConcurrency);

        hostBuilder.UseNServiceBus(endpointConfiguration);

        var host = hostBuilder.Build();

        await host.RunAsync();
    }

    #endregion
}
