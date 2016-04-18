using System;
using NServiceBus;

class Program
{

    static void Main()
    {
        Console.Title = "Samples.DelayedDelivery.Client";
        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.DelayedDelivery.Client");
        busConfiguration.UseSerialization<JsonSerializer>();
        busConfiguration.EnableInstallers();
        busConfiguration.UsePersistence<InMemoryPersistence>();

        using (IBus bus = Bus.Create(busConfiguration).Start())
        {
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
                    bus.Send("Samples.DelayedDelivery.Server", placeOrder);
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
