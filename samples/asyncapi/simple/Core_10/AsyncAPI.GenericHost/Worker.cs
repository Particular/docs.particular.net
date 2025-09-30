class Worker(IMessageSession messageSession, IHostApplicationLifetime appLifetime) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        try
        {
            Console.WriteLine("Press '1' to publish event one");
            Console.WriteLine("Press '2' to publish event two");
            Console.WriteLine("Press any other key to exit");

            while (true)
            {
                var key = Console.ReadKey();
                Console.WriteLine();

                var now = DateTime.UtcNow.ToString();
                switch (key.Key)
                {
                    case ConsoleKey.D1:                        
                        await messageSession.Publish(new SampleEventOne { SomeValue = now}, cancellationToken);

                        Console.WriteLine($"Published event 1 with {now}.");
                        break;
                    case ConsoleKey.D2:
                        await messageSession.Publish(new SampleEventTwo { SomeValue = now }, cancellationToken);

                        Console.WriteLine($"Published event 2 with {now}.");
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