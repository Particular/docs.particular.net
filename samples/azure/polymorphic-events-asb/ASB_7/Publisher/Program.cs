﻿using System;
using System.Threading.Tasks;
using Events;
using NServiceBus;
using NServiceBus.AzureServiceBus;
using NServiceBus.Features;

class Program
{
    static void Main()
    {
        MainAsync().GetAwaiter().GetResult();
    }

    static async Task MainAsync()
    {
        Console.Title = "Samples.ASB.Polymorphic.Publisher";
        var endpointConfiguration = new EndpointConfiguration("Samples.ASB.Polymorphic.Publisher");
        var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
        transport.UseTopology<EndpointOrientedTopology>();
        transport.ConnectionString(Environment.GetEnvironmentVariable("AzureServiceBus.ConnectionString"));
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.UseSerialization<JsonSerializer>();
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.DisableFeature<SecondLevelRetries>();

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);

        try
        {
            Console.WriteLine("Press '1' to publish the base event");
            Console.WriteLine("Press '2' to publish the derived event");
            Console.WriteLine("Press any other key to exit");

            while (true)
            {
                var key = Console.ReadKey();
                Console.WriteLine();

                var eventId = Guid.NewGuid();

                switch (key.Key)
                {
                    case ConsoleKey.D1:
                        await endpointInstance.Publish<BaseEvent>(e =>
                        {
                            e.EventId = eventId;
                        });
                        Console.WriteLine("BaseEvent sent. EventId: " + eventId);
                        break;

                    case ConsoleKey.D2:
                        await endpointInstance.Publish<DerivedEvent>(e =>
                        {
                            e.EventId = eventId;
                            e.Data = "more data";
                        });
                        Console.WriteLine("DerivedEvent sent. EventId: " + eventId);
                        break;

                    default:
                        return;
                }
            }

        }
        finally
        {
            await endpointInstance.Stop();
        }
    }
}