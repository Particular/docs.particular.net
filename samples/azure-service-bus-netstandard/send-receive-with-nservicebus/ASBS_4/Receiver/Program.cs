using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NServiceBus;

namespace Receiver
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
                    var endpointConfiguration = new EndpointConfiguration("Receiver");

                    var connectionString = context.Configuration.GetConnectionString("AzureServiceBusConnectionString");
                    var transport = endpointConfiguration.UseTransport(new AzureServiceBusTransport(connectionString));
                    endpointConfiguration.UseSerialization<SystemJsonSerializer>();

                    endpointConfiguration.AuditProcessedMessagesTo("audit");

                    // Operational scripting: https://docs.particular.net/transports/azure-service-bus/operational-scripting
                    endpointConfiguration.EnableInstallers();

                    return endpointConfiguration;
                })
                .Build();

            await host.RunAsync();
        }
    }
}
