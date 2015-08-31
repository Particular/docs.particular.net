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
        Console.WriteLine("Press any key to exit");

        while (true)
        {
            ConsoleKeyInfo key = Console.ReadKey();
            Console.WriteLine();

            switch (key.Key)
            {
                case ConsoleKey.E:
                    PublishEvent(bus);
                    continue;
                case ConsoleKey.D:
                    DeferMessage(bus);
                    continue;
            }
            return;
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