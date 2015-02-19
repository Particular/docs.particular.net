using System;
using MyMessages;
using MyMessages.Other;
using NServiceBus;

static class Program
{

    static void Main()
    {
        var busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.PubSub.MyPublisher");
        busConfiguration.UseSerialization<JsonSerializer>();
        busConfiguration.UsePersistence<InMemoryPersistence>();
        busConfiguration.EnableInstallers();
        var startableBus = Bus.Create(busConfiguration);
        using (var bus = startableBus.Start())
        {
            Start(bus);
        }
    }

    static void Start(IBus bus)
    {
        Console.WriteLine("This will publish IEvent, EventMessage, and AnotherEventMessage alternately.");
        Console.WriteLine("Press 'Enter' to publish a message.To exit, Ctrl + C");
        #region PublishLoop
        var nextEventToPublish = 0;
        while (Console.ReadLine() != null)
        {
            var eventId = Guid.NewGuid();
            switch (nextEventToPublish)
            {
                case 0:
                    bus.Publish<IMyEvent>(m =>
                    {
                        m.EventId = eventId;
                        m.Time = DateTime.Now.Second > 30 ? (DateTime?) DateTime.Now : null;
                        m.Duration = TimeSpan.FromSeconds(99999D);
                    });
                    nextEventToPublish = 1;
                    break;
                case 1:
                    var eventMessage = new EventMessage
                    {
                        EventId = eventId,
                        Time = DateTime.Now.Second > 30 ? (DateTime?) DateTime.Now : null,
                        Duration = TimeSpan.FromSeconds(99999D)
                    };
                    bus.Publish(eventMessage);
                    nextEventToPublish = 2;
                    break;
                default:
                    var anotherEventMessage = new AnotherEventMessage
                    {
                        EventId = eventId,
                        Time = DateTime.Now.Second > 30 ? (DateTime?) DateTime.Now : null,
                        Duration = TimeSpan.FromSeconds(99999D)
                    };
                    bus.Publish(anotherEventMessage);
                    nextEventToPublish = 0;
                    break;
            }

            Console.WriteLine("Published event with Id {0}.", eventId);

            Console.WriteLine("==========================================================================");
        }
        #endregion
    }

}