using System;
using NServiceBus;
using NServiceBus.Installation.Environments;

class Program
{
    static void Main()
    {
        Console.Title = "Samples.DelayedDelivery.Client";
        Configure.Serialization.Json();
        var configure = Configure.With();
        configure.Log4Net();
        configure.DefineEndpointName("Samples.DelayedDelivery.Client");
        configure.DefaultBuilder();
        configure.InMemorySagaPersister();
        configure.UseInMemoryTimeoutPersister();
        configure.InMemorySubscriptionStorage();
        configure.UseTransport<Msmq>();
        using (var startableBus = configure.UnicastBus().CreateBus())
        {
            var bus = startableBus.Start(() => configure.ForInstallationOn<Windows>().Install());
            SendOrder(bus);
        }
    }

    static void SendOrder(IBus bus)
    {
        Console.WriteLine("Press '1' to send PlaceOrder - defer message handling");
        Console.WriteLine("Press '2' to send PlaceDelayedOrder - defer message delivery");
        Console.WriteLine("Press enter key to exit");

        while (true)
        {
            var key = Console.ReadKey();
            Console.WriteLine();
            var id = Guid.NewGuid();

            switch (key.Key)
            {
                case ConsoleKey.D1:
                    #region SendOrder
                    var placeOrder = new PlaceOrder
                    {
                        Product = "New shoes",
                        Id = id
                    };
                    bus.Send("Samples.DelayedDelivery.Server", placeOrder);
                    Console.WriteLine($"[Defer Message Handling] Sent a PlaceOrder message with id: {id.ToString("N")}");
                    #endregion
                    continue;
                case ConsoleKey.D2:
                    #region DeferOrder
                    var placeDelayedOrder = new PlaceDelayedOrder
                    {
                        Product = "New shoes",
                        Id = id
                    };
                    bus.Defer(TimeSpan.FromSeconds(5), placeDelayedOrder);
                    Console.WriteLine($"[Defer Message Delivery] Deferred a PlaceDelayedOrder message with id: {id.ToString("N")}");
                    #endregion
                    continue;
                case ConsoleKey.Enter:
                    return;
                default:
                    return;
            }
        }

    }
}
