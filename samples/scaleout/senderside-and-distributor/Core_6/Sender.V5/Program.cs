using System;
using System.Linq;
using NServiceBus;

class Program
{
    static void Main()
    {
        Console.Title = "Sender.V5";

        var distributor = "Samples.Scaleout.Distributor";
        var busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.Scaleout.Sender.V5");
        busConfiguration.EnableInstallers();
        busConfiguration.UsePersistence<InMemoryPersistence>();
        var conventions = busConfiguration.Conventions();
        conventions.DefiningMessagesAs(
            type =>
            {
                return type.GetInterfaces().Contains(typeof(IMessage));
            });

        using (var bus = Bus.Create(busConfiguration).Start())
        {
            Console.WriteLine("Press 'A' to send a valid message or 'B' to send an invalid one.");
            Console.WriteLine("Press any other key to exit.");
            while (true)
            {
                var key = Console.ReadKey();
                Console.WriteLine();

                var orderId = Guid.NewGuid();
                if (key.Key == ConsoleKey.A)
                {
                    var placeOrder = new PlaceOrder
                    {
                        OrderId = orderId
                    };
                    bus.Send(distributor, placeOrder);

                    Console.WriteLine($"Sent PlacedOrder command with order id {orderId}");
                }
                else if (key.Key == ConsoleKey.B)
                {
                    var placeInvalidOrder = new PlaceInvalidOrder
                    {
                        OrderId = orderId
                    };
                    bus.Send(distributor, placeInvalidOrder);

                    Console.WriteLine($"Sent PlacedOrder command with order id {orderId}");
                }
                else
                {
                    return;
                }
            }
        }
    }
}