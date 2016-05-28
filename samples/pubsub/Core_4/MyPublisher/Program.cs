using System;
using NServiceBus;
using NServiceBus.Installation.Environments;

class Program
{
    static void Main()
    {
        Console.Title = "Samples.PubSub.MyPublisher";
        Configure.Serialization.Json();
        var configure = Configure.With();
        configure.Log4Net();
        configure.DefineEndpointName("Samples.PubSub.MyPublisher");
        configure.DefaultBuilder();
        configure.InMemorySagaPersister();
        configure.UseInMemoryTimeoutPersister();
        configure.InMemorySubscriptionStorage();
        configure.UseTransport<Msmq>();
        using (var startableBus = configure.UnicastBus().CreateBus())
        {
            var bus = startableBus.Start(() => configure.ForInstallationOn<Windows>().Install());
            Start(bus);
        }
    }

    static void Start(IBus bus)
    {
        Console.WriteLine("Press '1' to publish IEvent");
        Console.WriteLine("Press '2' to publish EventMessage");
        Console.WriteLine("Press '3' to publish AnotherEventMessage");
        Console.WriteLine("Press any key other key to exit");
        #region PublishLoop
        while (true)
        {
            var key = Console.ReadKey();
            Console.WriteLine();

            var eventId = Guid.NewGuid();
            switch (key.Key)
            {
                case ConsoleKey.D1:
                    bus.Publish<IMyEvent>(m =>
                    {
                        m.EventId = eventId;
                        m.Time = DateTime.Now.Second > 30 ? (DateTime?)DateTime.Now : null;
                        m.Duration = TimeSpan.FromSeconds(99999D);
                    });
                    Console.WriteLine($"Published IMyEvent with Id {eventId}.");
                    continue;
                case ConsoleKey.D2:
                    var eventMessage = new EventMessage
                    {
                        EventId = eventId,
                        Time = DateTime.Now.Second > 30 ? (DateTime?)DateTime.Now : null,
                        Duration = TimeSpan.FromSeconds(99999D)
                    };
                    bus.Publish(eventMessage);
                    Console.WriteLine($"Published EventMessage with Id {eventId}.");
                    continue;
                case ConsoleKey.D3:
                    var anotherEventMessage = new AnotherEventMessage
                    {
                        EventId = eventId,
                        Time = DateTime.Now.Second > 30 ? (DateTime?)DateTime.Now : null,
                        Duration = TimeSpan.FromSeconds(99999D)
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