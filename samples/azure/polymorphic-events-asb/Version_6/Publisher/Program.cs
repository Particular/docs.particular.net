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
        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.ASB.Polymorphic.Publisher");
        busConfiguration.UseTransport<AzureServiceBusTransport>()
            .UseTopology<StandardTopology>()
            .ConnectionString(Environment.GetEnvironmentVariable("AzureServiceBus.ConnectionString"));
        busConfiguration.UsePersistence<InMemoryPersistence>();
        busConfiguration.UseSerialization<JsonSerializer>();
        busConfiguration.EnableInstallers();
        busConfiguration.SendFailedMessagesTo("error");
        busConfiguration.DisableFeature<SecondLevelRetries>();

        IEndpointInstance endpoint = await Endpoint.Start(busConfiguration);
        IBusSession bus = endpoint.CreateBusSession();

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
                        await bus.Publish<BaseEvent>(e =>
                        {
                            e.EventId = eventId;
                        });
                        Console.WriteLine("BaseEvent sent. EventId: " + eventId);
                        break;

                    case ConsoleKey.D2:
                        await bus.Publish<DerivedEvent>(e =>
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