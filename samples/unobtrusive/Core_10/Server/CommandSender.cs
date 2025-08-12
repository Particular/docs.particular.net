using Events;

class CommandSender
{
    public static async Task Start(IMessageSession messageSession)
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
                    await PublishEvent(messageSession);
                    continue;
            }
            return;
        }
    }

    static Task PublishEvent(IMessageSession messageSession)
    {
        var eventId = Guid.NewGuid();

        Console.WriteLine($"Event published, id: {eventId}");
        var myEvent = new MyEvent
        {
            EventId = eventId
        };
        return messageSession.Publish(myEvent);
    }
}