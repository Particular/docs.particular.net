using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

static class Program
{

    static void Main()
    {
        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
    {
        LogManager.Use<DefaultFactory>().Level(LogLevel.Info);
        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.PubSub.MyPublisher");
        busConfiguration.UseSerialization<JsonSerializer>();
        busConfiguration.UsePersistence<InMemoryPersistence>();
        busConfiguration.SendFailedMessagesTo("error");
        busConfiguration.EnableInstallers();

        IEndpointInstance endpoint = await Endpoint.Start(busConfiguration);
        try
        {
            Start(endpoint);
        }
        finally
        {
            await endpoint.Stop();
        }
    }

    static void Start(IBusSession busSession)
    {
        Console.WriteLine("Press '1' to publish IEvent");
        Console.WriteLine("Press '2' to publish EventMessage");
        Console.WriteLine("Press '3' to publish AnotherEventMessage");
        Console.WriteLine("Press 'Enter' to publish a message.To exit, Ctrl + C");
        #region PublishLoop
        while (true)
        {
            ConsoleKeyInfo key = Console.ReadKey();
            Console.WriteLine();

            Guid eventId = Guid.NewGuid();
            switch (key.Key)
            {
                case ConsoleKey.D1:
                    busSession.Publish<IMyEvent>(m =>
                    {
                        m.EventId = eventId;
                        m.Time = DateTime.Now.Second > 30 ? (DateTime?) DateTime.Now : null;
                        m.Duration = TimeSpan.FromSeconds(99999D);
                    });
                    Console.WriteLine("Published IMyEvent with Id {0}.", eventId);
                    continue;
                case ConsoleKey.D2:
                    EventMessage eventMessage = new EventMessage
                    {
                        EventId = eventId,
                        Time = DateTime.Now.Second > 30 ? (DateTime?) DateTime.Now : null,
                        Duration = TimeSpan.FromSeconds(99999D)
                    };
                    busSession.Publish(eventMessage);
                    Console.WriteLine("Published EventMessage with Id {0}.", eventId);
                    continue;
                case ConsoleKey.D3:
                    AnotherEventMessage anotherEventMessage = new AnotherEventMessage
                    {
                        EventId = eventId,
                        Time = DateTime.Now.Second > 30 ? (DateTime?) DateTime.Now : null,
                        Duration = TimeSpan.FromSeconds(99999D)
                    };
                    busSession.Publish(anotherEventMessage);
                    Console.WriteLine("Published AnotherEventMessage with Id {0}.", eventId);
                    continue;
                default:
                    return;
            }
        }
        #endregion
    }

}