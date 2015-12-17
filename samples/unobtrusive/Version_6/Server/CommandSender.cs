using System;
using System.Threading.Tasks;
using Events;
using NServiceBus;

class CommandSender
{

    public static async Task Start(IBusContext busContext)
    {
        Console.WriteLine("Press 'E' to publish an event");
        Console.WriteLine("Press any key to exit");

        while (true)
        {
            ConsoleKeyInfo key = Console.ReadKey();
            Console.WriteLine();

            switch (key.Key)
            {
                case ConsoleKey.E:
                    await PublishEvent(busContext);
                    continue;
            }
            return;
        }
    }

    static async Task PublishEvent(IBusContext busContext)
    {
        Guid eventId = Guid.NewGuid();

        await busContext.Publish<IMyEvent>(m =>
        {
            m.EventId = eventId;
        });
        Console.WriteLine("Event published, id: " + eventId);

    }

}