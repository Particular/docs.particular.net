using System;
using NServiceBus;
using NServiceBus.Installation.Environments;

class Program
{
    static void Main()
    {
        Console.Title = "Samples.PubSub.Publisher";
        Configure.Serialization.Json();
        var configure = Configure.With();
        configure.Log4Net();
        configure.DefineEndpointName("Samples.PubSub.Publisher");
        configure.DefaultBuilder();
        configure.InMemorySagaPersister();
        configure.UseInMemoryTimeoutPersister();
        configure.InMemorySubscriptionStorage();
        configure.UseTransport<Msmq>();
        using (var startableBus = configure.UnicastBus().CreateBus())
        {
            var bus = startableBus.Start(() => configure.ForInstallationOn<Windows>().Install());
            Start(bus);
        }
    }

    static void Start(IBus bus)
    {
        Console.WriteLine("Press '1' to publish OrderReceived event");
        Console.WriteLine("Press any key other key to exit");
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