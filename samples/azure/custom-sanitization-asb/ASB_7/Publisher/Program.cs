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
        var endpointConfiguration = new EndpointConfiguration("Samples.ASB.Serialization.Publisher");
        var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
        transport.ConnectionString(Environment.GetEnvironmentVariable("AzureServiceBus.ConnectionString"));
        transport.UseTopology<ForwardingTopology>();
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.UseSerialization<JsonSerializer>();
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.DisableFeature<SecondLevelRetries>();

        IEndpointInstance endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);

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
                var someEvent = new SomeEvent
                {
                    EventId = eventId
                };
                await endpointInstance.Publish(someEvent)
                    .ConfigureAwait(false);
                Console.WriteLine($"SomeEvent sent. EventId: {eventId}");
            }
        }
        finally
        {
            await endpointInstance.Stop()
                .ConfigureAwait(false);
        }
    }
}