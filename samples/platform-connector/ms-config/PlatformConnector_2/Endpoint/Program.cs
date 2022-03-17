using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;

public class Program
{
    public static Task Main()
    {
        Console.Title = "Endpoint";

        return new HostBuilder()
            .ConfigureHostConfiguration(config =>
            {
                #region addConfigFile
                config.AddJsonFile("appsettings.json");
                #endregion
            })
            .UseNServiceBus(hostContext =>
            {
                var endpointConfiguration = new EndpointConfiguration("Endpoint");

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
            }).RunConsoleAsync();
    }
}