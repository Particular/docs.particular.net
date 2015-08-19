using System;
using NServiceBus;
using Orders.Commands;
using Orders.Messages;

class Program
{

    static void Main()
    {

        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Orders.Sender");
        busConfiguration.EnableInstallers();
        busConfiguration.UsePersistence<InMemoryPersistence>();

        using (IBus bus = Bus.Create(busConfiguration).Start())
        {
            Console.WriteLine("Press 'Enter' to send a message.");
            Console.WriteLine("Press any other key to exit.");
            int counter = 0;
            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey();
                Console.WriteLine();

                if (key.Key != ConsoleKey.Enter)
                {
                    return;
                }
                counter++;
                PlaceOrder placeOrder = new PlaceOrder
                {
                    OrderId = "order" + counter
                };
                bus.Send(placeOrder).Register(PlaceOrderReturnCodeHandler);
                Console.WriteLine("Sent PlacedOrder command with order id [{0}].", placeOrder.OrderId);
            }
        }
    }

    static void PlaceOrderReturnCodeHandler(CompletionResult completionResult)
    {
        Console.WriteLine("Received [{0}] Return code for Placing Order.", Enum.GetName(typeof(PlaceOrderStatus), completionResult.ErrorCode));
    }

}