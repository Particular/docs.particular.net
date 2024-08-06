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
            var builder = Host.CreateApplicationBuilder(args);

            builder.Logging.AddConfiguration(builder.Configuration.GetSection("Logging"));
            builder.Logging.AddConsole();

            var endpointConfiguration = new EndpointConfiguration("Receiver");

            var connectionString = builder.Configuration.GetConnectionString("AzureServiceBusConnectionString");
            var transport = endpointConfiguration.UseTransport(new AzureServiceBusTransport(connectionString));
            endpointConfiguration.UseSerialization<SystemJsonSerializer>();

            endpointConfiguration.AuditProcessedMessagesTo("audit");

            // Operational scripting: https://docs.particular.net/transports/azure-service-bus/operational-scripting
            endpointConfiguration.EnableInstallers();

            builder.UseNServiceBus(endpointConfiguration);

            var host = builder.Build();
            await host.RunAsync();
        }
    }
}
