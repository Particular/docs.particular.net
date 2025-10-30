using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NServiceBus;
using Shared;

var builder = Host.CreateApplicationBuilder(args);
builder.Logging.AddConsole();

var endpointConfiguration = new EndpointConfiguration("Samples.TopologyMigration.Publisher");

var connectionString = builder.Configuration.GetConnectionString("AzureServiceBusConnectionString")!;

#pragma warning disable CS0618 // Type or member is obsolete

//Step 0: Using the topology that supports migration and single-topic event delivery path
var topology = TopicTopology.MigrateFromSingleDefaultTopic();
topology.EventToMigrate<MyEvent>();

#pragma warning restore CS0618 // Type or member is obsolete

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

var host = builder.Build();
await host.StartAsync();

var messageSession = host.Services.GetRequiredService<IMessageSession>();

Console.WriteLine("Publishing messages... Press [Ctrl] + [C] to cancel");

using (var cts = new CancellationTokenSource())
{
    Console.CancelKeyPress += (s, e) =>
    {
        Console.WriteLine("Cancellation Requested...");
        cts.Cancel();
        e.Cancel = true;
    };

    try
    {
        var number = 0;

        while (true)
        {
            var myEvent = new MyEvent
            {
                Content = $"MyEvent {number++}",
                PublishedOnUtc = DateTime.UtcNow
            };
            await messageSession.Publish(myEvent, cts.Token);

            Console.WriteLine($"Published MyEvent {{ Content = {myEvent.Content}, PublishedOnUtc = {myEvent.PublishedOnUtc} }}");

            await Task.Delay(1000, cts.Token);
        }
    }
    catch (OperationCanceledException) when (cts.Token.IsCancellationRequested)
    {
        // graceful shutdown
    }
}

await host.StopAsync();
