using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NServiceBus;
using Shared;

namespace Sender
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var host = Host.CreateDefaultBuilder(args)
                .ConfigureLogging((context, logging) =>
                {
                    logging.AddConfiguration(context.Configuration.GetSection("Logging"));

                    logging.AddConsole();
                })
                .UseConsoleLifetime()
                .UseNServiceBus(context =>
                {
                    var endpointConfiguration = new EndpointConfiguration("Sender");

                    var connectionString = context.Configuration.GetConnectionString("AzureServiceBusConnectionString");
                    var routing = endpointConfiguration.UseTransport(new AzureServiceBusTransport(connectionString));

                    endpointConfiguration.AuditProcessedMessagesTo("audit");
                    routing.RouteToEndpoint(typeof(Ping), "Receiver");

                    // Operational scripting: https://docs.particular.net/transports/azure-service-bus/operational-scripting
                    endpointConfiguration.EnableInstallers();

                    return endpointConfiguration;
                })
                .ConfigureServices(services => services.AddHostedService<SenderWorker>())
                .Build();

            await host.RunAsync();
        }
    }
}
