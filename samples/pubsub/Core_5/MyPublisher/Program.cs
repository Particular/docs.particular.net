using System;
using NServiceBus;
using NServiceBus.Logging;

static class Program
{

    static void Main()
    {
        Console.Title = "Samples.PubSub.MyPublisher";
        LogManager.Use<DefaultFactory>().Level(LogLevel.Info);
        var busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.PubSub.MyPublisher");
        busConfiguration.UseSerialization<JsonSerializer>();
        busConfiguration.UsePersistence<InMemoryPersistence>();
        busConfiguration.EnableInstallers();
        using (var bus = Bus.Create(busConfiguration).Start())
        {
            Start(bus);
        }
    }

    static void Start(IBus bus)
    {
        Console.WriteLine("Press '1' to publish IEvent");
        Console.WriteLine("Press '2' to publish EventMessage");
        Console.WriteLine("Press '3' to publish AnotherEventMessage");
        Console.WriteLine("Press any other key to exit");
        #region PublishLoop
        while (true)
        {
            var key = Console.ReadKey();
            Console.WriteLine();

            var eventId = Guid.NewGuid();
            var time = DateTime.Now.Second > 30 ? (DateTime?) DateTime.Now : null;
            var duration = TimeSpan.FromSeconds(99999D);
            switch (key.Key)
            {
                case ConsoleKey.D1:
                    bus.Publish<IMyEvent>(m =>
                    {
                        m.EventId = eventId;
                        m.Time = time;
                        m.Duration = duration;
                    });
                    Console.WriteLine($"Published IMyEvent with Id {eventId}.");
                    continue;
                case ConsoleKey.D2:
                    var eventMessage = new EventMessage
                    {
                        EventId = eventId,
                        Time = time,
                        Duration = duration
                    };
                    bus.Publish(eventMessage);
                    Console.WriteLine($"Published EventMessage with Id {eventId}.");
                    continue;
                case ConsoleKey.D3:
                    var anotherEventMessage = new AnotherEventMessage
                    {
                        EventId = eventId,
                        Time = time,
                        Duration = duration
                    };
                    bus.Publish(anotherEventMessage);
                    Console.WriteLine($"Published AnotherEventMessage with Id {eventId}.");
                    continue;
                default:
                    return;
            }
        }
        #endregion
    }

}