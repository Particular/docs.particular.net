using System;
using NServiceBus;
using NServiceBus.Installation.Environments;

class Program
{
    static void Main()
    {
        Console.Title = "Samples.StepByStep.Client";
        Configure configure = Configure.With();
        configure.Log4Net();
        configure.DefineEndpointName("Samples.StepByStep.Client");
        configure.DefaultBuilder();
        configure.MsmqTransport();
        configure.InMemorySagaPersister();
        configure.RunTimeoutManagerWithInMemoryPersistence();
        configure.InMemorySubscriptionStorage();
        configure.JsonSerializer();
        configure.RunTimeoutManager();
        using (IStartableBus startableBus = configure.UnicastBus().CreateBus())
        {
            IBus bus = startableBus
                .Start(() => configure.ForInstallationOn<Windows>().Install());
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
            ConsoleKeyInfo key = Console.ReadKey();
            Console.WriteLine();
            Guid id = Guid.NewGuid();

            switch (key.Key)
            {
                case ConsoleKey.D1:
                    #region SendOrder
                    PlaceOrder placeOrder = new PlaceOrder
                    {
                        Product = "New shoes",
                        Id = id
                    };
                    bus.Send("Samples.StepByStep.Server", placeOrder);
                    Console.WriteLine("[Defer Message Handling] Sent a new PlaceOrder message with id: {0}", id.ToString("N"));
                    #endregion
                    continue;
                case ConsoleKey.D2:
                    #region DeferOrder
                    PlaceDelayedOrder placeDelayedOrder = new PlaceDelayedOrder
                    {
                        Product = "New shoes",
                        Id = id
                    };
                    bus.Defer(TimeSpan.FromSeconds(5), placeDelayedOrder);
                    Console.WriteLine("[Defer Message Delivery] Deferred a new PlaceDelayedOrder message with id: {0}", id.ToString("N"));
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
