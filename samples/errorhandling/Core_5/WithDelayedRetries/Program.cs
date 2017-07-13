using System;
using NServiceBus;
using NServiceBus.Logging;

static class Program
{
    static void Main()
    {
        Console.Title = "Samples.ErrorHandling.WithDelayedRetries";
        LogManager.Use<DefaultFactory>()
            .Level(LogLevel.Warn);

        var busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.ErrorHandling.WithDelayedRetries");
        busConfiguration.UsePersistence<InMemoryPersistence>();
        busConfiguration.EnableInstallers();
        using (var bus = Bus.Create(busConfiguration).Start())
        {
            Console.WriteLine("Press enter to send a message that will throw an exception.");
            Console.WriteLine("Press any key to exit");

            while (true)
            {
                var key = Console.ReadKey();
                if (key.Key != ConsoleKey.Enter)
                {
                    return;
                }
                var myMessage = new MyMessage
                {
                    Id = Guid.NewGuid()
                };
                bus.SendLocal(myMessage);
            }
        }
    }
}