using System;
using Events;
using Messages;
using NServiceBus;

class CommandSender
{

    public static void Start(IBus bus)
    {
        Console.WriteLine("Press 'E' to publish an event");
        Console.WriteLine("Press 'D' to send a deferred message");
        Console.WriteLine("To exit, press Ctrl + C");

        while (true)
        {
            string cmd = Console.ReadKey().Key.ToString().ToLower();

            switch (cmd)
            {
                case "e":
                    PublishEvent(bus);
                    break;
                case "d":
                    DeferMessage(bus);
                    break;
            }
        }
    }

    static void DeferMessage(IBus bus)
    {
        bus.Defer(TimeSpan.FromSeconds(10), new DeferredMessage());
        Console.WriteLine();
        Console.WriteLine("{0} - {1}", DateTime.Now.ToLongTimeString(), "Sent a message that is deferred for 10 seconds");
    }

    static void PublishEvent(IBus bus)
    {
        Guid eventId = Guid.NewGuid();

        bus.Publish<IMyEvent>(m =>
        {
            m.EventId = eventId;
        });
        Console.WriteLine("Event published, id: " + eventId);

    }

}