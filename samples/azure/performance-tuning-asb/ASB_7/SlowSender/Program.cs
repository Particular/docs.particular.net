﻿using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using NServiceBus;

class Program
{

    const int NumberOfMessages = 100;

    static void Main()
    {
        MainAsync().GetAwaiter().GetResult();
    }

    static async Task MainAsync()
    {
        Console.Title = "Samples.ASB.Performance.SlowSender";
        var endpointConfiguration = new EndpointConfiguration("Samples.ASB.Performance.Sender");
        var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
        var connectionString = Environment.GetEnvironmentVariable("AzureServiceBus.ConnectionString");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new Exception("Could not read the 'AzureServiceBus.ConnectionString' environment variable. Check the sample prerequisites.");
        }
        transport.UseForwardingTopology();
        transport.ConnectionString(connectionString);
        var queues = transport.Queues();
        queues.EnablePartitioning(true);

        var receiverName = "Samples.ASB.Performance.Receiver";
        await EnsureReceiverQueueExists(receiverName, connectionString)
            .ConfigureAwait(false);
        var routing = transport.Routing();
        routing.RouteToEndpoint(typeof(SomeMessage), receiverName);

        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.UseSerialization<JsonSerializer>();
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.SendFailedMessagesTo("error");

        #region slow-send-config

        var factories = transport.MessagingFactories();
        factories.BatchFlushInterval(TimeSpan.Zero);
        factories.NumberOfMessagingFactoriesPerNamespace(1);
        transport.NumberOfClientsPerEntity(1);

        #endregion

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);

        await Console.Out.WriteLineAsync("Press 'e' to send a large amount of messages")
            .ConfigureAwait(false);
        await Console.Out.WriteLineAsync("Press any other key to exit")
            .ConfigureAwait(false);

        while (true)
        {
            var key = Console.ReadKey();
            await Console.Out.WriteLineAsync()
                .ConfigureAwait(false);

            var eventId = Guid.NewGuid();

            if (key.Key != ConsoleKey.E)
            {
                break;
            }

            var stopwatch = Stopwatch.StartNew();

            #region slow-send
            for (var i = 0; i < NumberOfMessages; i++)
            {
                await Console.Out.WriteLineAsync("Sending a message...");

                // by awaiting each individual send, no client side batching can take place
                // latency is incurred for each send and thus lowest performance possible
                await endpointInstance.Send(new SomeMessage())
                    .ConfigureAwait(false);
            }
            #endregion

            stopwatch.Stop();
            var elapsedSeconds = stopwatch.ElapsedTicks / (double)Stopwatch.Frequency;
            var msgsPerSecond = NumberOfMessages / elapsedSeconds;
            await Console.Out.WriteLineAsync($"sending {NumberOfMessages} messages took {stopwatch.ElapsedMilliseconds} milliseconds, or {msgsPerSecond} messages per second")
                .ConfigureAwait(false);

        }
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }

    static async Task EnsureReceiverQueueExists(string receiverPath, string connectionString)
    {
        var namespaceManager = NamespaceManager.CreateFromConnectionString(connectionString);
        if (!await namespaceManager.QueueExistsAsync(receiverPath)
            .ConfigureAwait(false))
        {
            var queueDescription = new QueueDescription(receiverPath)
            {
                EnablePartitioning = true,
                EnableBatchedOperations = true
            };
            await namespaceManager.CreateQueueAsync(queueDescription)
                .ConfigureAwait(false);
        }
    }

}