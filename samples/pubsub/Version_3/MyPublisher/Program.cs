using System;
using NServiceBus;
using NServiceBus.Installation.Environments;

class Program
{
    static void Main()
    {
        Console.Title = "Samples.PubSub.MyPublisher";
        Configure configure = Configure.With();
        configure.Log4Net();
        configure.DefineEndpointName("Samples.PubSub.MyPublisher");
        configure.DefaultBuilder();
        configure.MsmqTransport();
        configure.InMemorySagaPersister();
        configure.RunTimeoutManagerWithInMemoryPersistence();
        configure.InMemorySubscriptionStorage();
        configure.JsonSerializer();
        using (IStartableBus startableBus = configure.UnicastBus().CreateBus())
        {
            IBus bus = startableBus
                .Start(() => configure.ForInstallationOn<Windows>().Install());
            Start(bus);
        }
    }

    static void Start(IBus bus)
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
                    bus.Publish<IMyEvent>(m =>
                    {
                        m.EventId = eventId;
                        m.Time = DateTime.Now.Second > 30 ? (DateTime?)DateTime.Now : null;
                        m.Duration = TimeSpan.FromSeconds(99999D);
                    });
                    Console.WriteLine("Published IMyEvent with Id {0}.", eventId);
                    continue;
                case ConsoleKey.D2:
                    EventMessage eventMessage = new EventMessage
                    {
                        EventId = eventId,
                        Time = DateTime.Now.Second > 30 ? (DateTime?)DateTime.Now : null,
                        Duration = TimeSpan.FromSeconds(99999D)
                    };
                    bus.Publish(eventMessage);
                    Console.WriteLine("Published EventMessage with Id {0}.", eventId);
                    continue;
                case ConsoleKey.D3:
                    AnotherEventMessage anotherEventMessage = new AnotherEventMessage
                    {
                        EventId = eventId,
                        Time = DateTime.Now.Second > 30 ? (DateTime?)DateTime.Now : null,
                        Duration = TimeSpan.FromSeconds(99999D)
                    };
                    bus.Publish(anotherEventMessage);
                    Console.WriteLine("Published AnotherEventMessage with Id {0}.", eventId);
                    continue;
                default:
                    return;
            }
        }
        #endregion
    }
}