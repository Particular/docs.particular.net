using System;
using Events;
using NServiceBus;

class CommandSender
{

    public static void Start(IBus bus)
    {
        Console.WriteLine("Press 'E' to publish an event");
        Console.WriteLine("Press any key to exit");

        while (true)
        {
            var key = Console.ReadKey();
            Console.WriteLine();

            switch (key.Key)
            {
                case ConsoleKey.E:
                    PublishEvent(bus);
                    continue;
            }
            return;
        }
    }

    static void PublishEvent(IBus bus)
    {
        var eventId = Guid.NewGuid();

        bus.Publish<IMyEvent>(m =>
        {
            m.EventId = eventId;
        });
        Console.WriteLine($"Event published, id: {eventId}");
    }

}