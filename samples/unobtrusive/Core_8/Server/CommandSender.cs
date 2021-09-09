using System;
using System.Threading.Tasks;
using Events;
using NServiceBus;

class CommandSender
{

    public static async Task Start(IEndpointInstance endpointInstance)
    {
        Console.WriteLine("Press 'E' to publish an event");
        Console.WriteLine("Press any other key to exit");

        while (true)
        {
            var key = Console.ReadKey();
            Console.WriteLine();

            switch (key.Key)
            {
                case ConsoleKey.E:
                    await PublishEvent(endpointInstance)
                        .ConfigureAwait(false);
                    continue;
            }
            return;
        }
    }

    static Task PublishEvent(IEndpointInstance endpointInstance)
    {
        var eventId = Guid.NewGuid();

        Console.WriteLine($"Event published, id: {eventId}");
        var myEvent = new MyEvent
        {
            EventId = eventId
        };
        return endpointInstance.Publish(myEvent);
    }
}