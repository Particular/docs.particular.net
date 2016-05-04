using System;
using System.Threading.Tasks;
using Shared.Messages.In.A.Deep.Nested.Namespace.Nested.Events;
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
        Console.Title = "Samples.ASB.Serialization.Publisher";
        EndpointConfiguration endpointConfiguration = new EndpointConfiguration("Samples.ASB.Serialization.Publisher");
        var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
        transport.ConnectionString(Environment.GetEnvironmentVariable("AzureServiceBus.ConnectionString"));
        transport.UseTopology<ForwardingTopology>();
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.UseSerialization<JsonSerializer>();
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.DisableFeature<SecondLevelRetries>();

        IEndpointInstance endpoint = await Endpoint.Start(endpointConfiguration);

        try
        {
            Console.WriteLine("Press 'e' to publish an event");
            Console.WriteLine("Press any other key to exit");

            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey();
                Console.WriteLine();

                Guid eventId = Guid.NewGuid();

                if (key.Key == ConsoleKey.E)
                {
                    await endpoint.Publish<SuperDuperEvent>(e =>
                    {
                        e.EventId = eventId;
                    });
                    Console.WriteLine("SuperDuperEvent sent. EventId: " + eventId);
                }
                else
                {
                    break;
                }
            }

        }
        finally
        {
            await endpoint.Stop();
        }
    }
}