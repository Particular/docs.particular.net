#pragma warning disable CS0618 // Type or member is obsolete

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NServiceBus;
using Publisher;
using Shared;

var builder = Host.CreateApplicationBuilder(args);
builder.Logging.AddConsole();

var endpointConfiguration = new EndpointConfiguration("Samples.TopologyMigration.Publisher");

var connectionString = builder.Configuration.GetConnectionString("AzureServiceBusConnectionString")!;

//Step 0: Using the topology that supports migration and single-topic event delivery path
var topology = TopicTopology.MigrateFromSingleDefaultTopic();
topology.EventToMigrate<MyEvent>();

//Step 2: Switch to topic-per-event delivery path
//var topology = TopicTopology.MigrateFromSingleDefaultTopic();
//topology.MigratedPublishedEvent<MyEvent>();

//Step 3: Switch to topic-per-event topology once all events have been migrated
//var topology = TopicTopology.Default;

var routing = endpointConfiguration.UseTransport(new AzureServiceBusTransport(connectionString, topology));
endpointConfiguration.UseSerialization<SystemJsonSerializer>();

// Operational scripting: https://docs.particular.net/transports/azure-service-bus/operational-scripting
endpointConfiguration.EnableInstallers();

builder.UseNServiceBus(endpointConfiguration);

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
await host.RunAsync();

#pragma warning restore CS0618 // Type or member is obsolete