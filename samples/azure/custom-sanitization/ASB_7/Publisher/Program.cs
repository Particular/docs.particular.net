using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.AzureServiceBus;
using NServiceBus.Features;
using Shared.Messages.In.A.Deep.Nested.Namespace.Nested.Events;

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

                if (key.Key != ConsoleKey.E)
                {
                    break;
                }
                await endpoint.Publish(new SomeEvent { EventId = eventId });
                Console.WriteLine("SomeEvent sent. EventId: " + eventId);
            }
        }
        finally
        {
            await endpoint.Stop();
        }
    }
}