using System;
using NServiceBus;
using NServiceBus.Logging;

static class Program
{
    static void Main()
    {
        Console.Title = "Samples.PubSub.Publisher";
        LogManager.Use<DefaultFactory>().Level(LogLevel.Info);
        var busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.PubSub.Publisher");
        busConfiguration.UseSerialization<JsonSerializer>();
        busConfiguration.UsePersistence<InMemoryPersistence>();
        busConfiguration.EnableInstallers();
        using (var bus = Bus.Create(busConfiguration).Start())
        {
            Start(bus);
        }
    }

    static void Start(IBus bus)
    {
        Console.WriteLine("Press '1' to publish OrderReceived event");
        Console.WriteLine("Press any other key to exit");
        #region PublishLoop
        while (true)
        {
            var key = Console.ReadKey();
            Console.WriteLine();
            var orderId = Guid.NewGuid();

            if (key.Key == ConsoleKey.D1)
            {
                bus.Publish<OrderReceived>(m => { m.OrderId = orderId; });
                Console.WriteLine($"Published the OrderReceived event with OrderId {orderId}.");
            }
            else
            {
                return;
            }
        }
        #endregion
    }

}