using System;
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
        EndpointConfiguration endpointConfiguration = new EndpointConfiguration();
        endpointConfiguration.EndpointName("Samples.ASB.Polymorphic.Publisher");
        endpointConfiguration.UseTransport<AzureServiceBusTransport>()
            .UseTopology<StandardTopology>()
            .ConnectionString(Environment.GetEnvironmentVariable("AzureServiceBus.ConnectionString"));
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.UseSerialization<JsonSerializer>();
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.DisableFeature<SecondLevelRetries>();

        IEndpointInstance endpoint = await Endpoint.Start(endpointConfiguration);

        try
        {
            Console.WriteLine("Press '1' to publish the base event");
            Console.WriteLine("Press '2' to publish the derived event");
            Console.WriteLine("Press any other key to exit");

            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey();
                Console.WriteLine();

                Guid eventId = Guid.NewGuid();

                switch (key.Key)
                {
                    case ConsoleKey.D1:
                        await endpoint.Publish<BaseEvent>(e =>
                        {
                            e.EventId = eventId;
                        });
                        Console.WriteLine("BaseEvent sent. EventId: " + eventId);
                        break;

                    case ConsoleKey.D2:
                        await endpoint.Publish<DerivedEvent>(e =>
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
            await endpoint.Stop();
        }
    }
}