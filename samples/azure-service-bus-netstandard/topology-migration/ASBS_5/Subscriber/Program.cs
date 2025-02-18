#pragma warning disable CS0618 // Type or member is obsolete

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NServiceBus;
using Shared;

var builder = Host.CreateApplicationBuilder(args);
builder.Logging.AddConsole();

var endpointConfiguration = new EndpointConfiguration("Samples.TopologyMigration.Subscriber");

var connectionString = builder.Configuration.GetConnectionString("AzureServiceBusConnectionString")!;

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

#pragma warning restore CS0618 // Type or member is obsolete