﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using NServiceBus;

class Program
{

    const int NumberOfMessages = 10000;

    static void Main()
    {
        MainAsync().GetAwaiter().GetResult();
    }

    static async Task MainAsync()
    {
        Console.Title = "Samples.ASB.Performance.Sender1";
        var endpointConfiguration = new EndpointConfiguration("Samples.ASB.Performance.Sender");
        var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
        var connectionString = Environment.GetEnvironmentVariable("AzureServiceBus.ConnectionString");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new Exception("Could not read the 'AzureServiceBus.ConnectionString' environment variable. Check the sample prerequisites.");
        }
        transport.UseTopology<ForwardingTopology>();
        transport.ConnectionString(connectionString);

        transport.Routing().RouteToEndpoint(typeof(SomeMessage), "Samples.ASB.Performance.Receiver");

        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.UseSerialization<JsonSerializer>();
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.SendFailedMessagesTo("error");
       
        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);

        try
        {
            Console.WriteLine("Press 'e' to send a large amount of messages");
            Console.WriteLine("Press any other key to exit");

            while (true)
            {
                var key = Console.ReadKey();
                Console.WriteLine();

                var eventId = Guid.NewGuid();

                if (key.Key != ConsoleKey.E)
                {
                    break;
                }

                var stopwatch = new Stopwatch();
                stopwatch.Start();
                var tasks = new List<Task>();
                for (var i = 0; i < NumberOfMessages; i++)
                {
                    Console.WriteLine("Sending a message...");
                    tasks.Add(endpointInstance.Send(new SomeMessage()));
                    //    await endpointInstance.Send(new MyMessage());
                }
                await Task.WhenAll(tasks);

                stopwatch.Stop();
                var elapsedSeconds = stopwatch.ElapsedTicks / (double)Stopwatch.Frequency;
                var msgsPerSecond = NumberOfMessages / elapsedSeconds;
                Console.WriteLine("sending " + NumberOfMessages + " messages took " + stopwatch.ElapsedMilliseconds + " milliseconds, or " + msgsPerSecond + " messages per second");

            }
        }
        finally
        {
            await endpointInstance.Stop()
                .ConfigureAwait(false);
        }
    }
}