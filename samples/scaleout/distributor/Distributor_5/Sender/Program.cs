using System;
using NServiceBus;

class Program
{

    static void Main()
    {
        Console.Title = "Samples.Scaleout.Sender";
        var busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.Scaleout.Sender");
        busConfiguration.EnableInstallers();
        busConfiguration.UsePersistence<InMemoryPersistence>();

        using (var bus = Bus.Create(busConfiguration).Start())
        {
            Console.WriteLine("Press 'Enter' to send a message.");
            Console.WriteLine("Press any other key to exit.");
            while (true)
            {
                var key = Console.ReadKey();
                Console.WriteLine();

                if (key.Key != ConsoleKey.Enter)
                {
                    return;
                }

                SendMessage(bus);
            }
        }
    }

    static void SendMessage(IBus bus)
    {
        #region sender

        var placeOrder = new PlaceOrder
        {
            OrderId = Guid.NewGuid()
        };
        bus.Send("Samples.Scaleout.Server", placeOrder);
        Console.WriteLine($"Sent PlacedOrder command with order id [{placeOrder.OrderId}].");

        #endregion
    }
}
