using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;

Console.Title = "Endpoint";

var hostBuilder = new HostBuilder()
    .ConfigureHostConfiguration(config =>
    {
        #region addConfigFile
        config.AddJsonFile("appsettings.json");
        #endregion
    })
    .UseNServiceBus(hostContext =>
    {
        var endpointConfiguration = new EndpointConfiguration("Endpoint");

        endpointConfiguration.UseSerialization<SystemJsonSerializer>();
        endpointConfiguration.UseTransport<LearningTransport>();
        endpointConfiguration.UsePersistence<NonDurablePersistence>();

        #region loadConnectionDetails
        var platformConnection = hostContext.Configuration
            .GetSection("ServicePlatformConfiguration")
            .Get<ServicePlatformConnectionConfiguration>();
        #endregion

        #region configureConnection
        endpointConfiguration.ConnectToServicePlatform(platformConnection);
        #endregion

        return endpointConfiguration;
    })
    .ConfigureServices((hostBuilderContext, services) =>
    {
        services.AddHostedService<BusinessMessageSimulator>();
    });

Console.WriteLine("Starting endpoint, use CTRL + C to stop");

await hostBuilder.RunConsoleAsync();