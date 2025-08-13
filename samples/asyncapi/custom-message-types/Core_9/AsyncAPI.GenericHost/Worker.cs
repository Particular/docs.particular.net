class Worker(IMessageSession messageSession, IHostApplicationLifetime appLifetime) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        try
        {
            var number = 0;

            Console.WriteLine("Press '1' to publish event 1");
            Console.WriteLine("Press '2' to publish event 2");
            Console.WriteLine("Press 's' to send local message");
            Console.WriteLine("Press any other key to exit");

            while (true)
            {
                var key = Console.ReadKey();
                Console.WriteLine();

                var now = DateTime.UtcNow.ToString();
                switch (key.Key)
                {
                    case ConsoleKey.D1:                        
                        await messageSession.Publish(new FirstEventThatIsBeingPublished { SomeValue = now, SomeOtherValue = now }, cancellationToken);

                        Console.WriteLine($"Published event 1 with {now}.");
                        break;
                    case ConsoleKey.D2:                        
                        await messageSession.Publish(new SecondEventThatIsBeingPublished { SomeValue = now, SomeOtherValue = now }, cancellationToken);

                        Console.WriteLine($"Published event 2 with {now}.");

                        break;
                    case ConsoleKey.S:
                        await messageSession.SendLocal(new MessageBeingSent { Number = number++ }, cancellationToken);

                        Console.WriteLine($"Sent message with {number}.");
                        break;
                    default:
                        appLifetime.StopApplication();
                        return;
                }
            }
        }
        catch (OperationCanceledException)
        {
            // graceful shutdown
        }
    }
}