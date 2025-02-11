using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NServiceBus;
using Shared;

namespace SubscriberV5
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Subscriber 1 running Azure Service Bus transport V5");
            var builder = Host.CreateApplicationBuilder(args);

            builder.Logging.AddConfiguration(builder.Configuration.GetSection("Logging"));
            builder.Logging.AddConsole();

            var endpointConfiguration = new EndpointConfiguration("Subscriber1");

            var connectionString = builder.Configuration.GetConnectionString("AzureServiceBusConnectionString");
            var topology = TopicTopology.MigrateFromSingleDefaultTopic();
            topology.EventToMigrate<MyEvent>();
            var routing = endpointConfiguration.UseTransport(new AzureServiceBusTransport(connectionString, topology));
            endpointConfiguration.UseSerialization<SystemJsonSerializer>();

            // Operational scripting: https://docs.particular.net/transports/azure-service-bus/operational-scripting
            endpointConfiguration.EnableInstallers();

            builder.UseNServiceBus(endpointConfiguration);

            var host = builder.Build();
            await host.RunAsync();
        }
    }
}