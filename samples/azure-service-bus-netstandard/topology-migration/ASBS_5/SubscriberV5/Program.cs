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
            var builder = Host.CreateApplicationBuilder(args);

            builder.Logging.AddConfiguration(builder.Configuration.GetSection("Logging"));
            builder.Logging.AddConsole();

            var endpointConfiguration = new EndpointConfiguration("Subscriber");

            var connectionString = builder.Configuration.GetConnectionString("AzureServiceBusConnectionString");

            //Step 0: Using the topology that supports migration and single-topic event delivery path
            var topology = TopicTopology.MigrateFromSingleDefaultTopic();
            topology.EventToMigrate<MyEvent>();

            //Step 1: Make sure topic-per-event path is created via installers or command line tool
            //var topology = TopicTopology.MigrateFromSingleDefaultTopic();
            //topology.MigratedSubscribedEvent<MyEvent>();

            //Step 3: Switch to topic-per-event topology once all events have been migrated
            //var topology = TopicTopology.Default;

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