using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NServiceBus;
using Shared;

namespace SubscriberV4
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Subscriber 2 running Azure Service Bus transport V4");
            var builder = Host.CreateApplicationBuilder(args);

            builder.Logging.AddConfiguration(builder.Configuration.GetSection("Logging"));
            builder.Logging.AddConsole();

            var endpointConfiguration = new EndpointConfiguration("Subscriber2");

            var connectionString = builder.Configuration.GetConnectionString("AzureServiceBusConnectionString");
            var routing = endpointConfiguration.UseTransport(new AzureServiceBusTransport(connectionString));
            endpointConfiguration.UseSerialization<SystemJsonSerializer>();

            // Operational scripting: https://docs.particular.net/transports/azure-service-bus/operational-scripting
            endpointConfiguration.EnableInstallers();

            builder.UseNServiceBus(endpointConfiguration);

            var host = builder.Build();
            await host.RunAsync();
        }
    }
}