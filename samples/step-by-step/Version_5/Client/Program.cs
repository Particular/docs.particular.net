using System;
using NServiceBus;

class Program
{

    static void Main()
    {
        Console.Title = "Samples.StepByStep.Client";
        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.StepByStep.Client");
        busConfiguration.UseSerialization<JsonSerializer>();
        busConfiguration.EnableInstallers();
        busConfiguration.UsePersistence<InMemoryPersistence>();

        using (IBus bus = Bus.Create(busConfiguration).Start())
        {
           SendOrder(bus);
        }
    }


    #region SendOrder
    static void SendOrder(IBus bus)
    {

        Console.WriteLine("Press enter to send a message");
        Console.WriteLine("Press any key to exit");

        while (true)
        {
            ConsoleKeyInfo key = Console.ReadKey();
            Console.WriteLine();

            if (key.Key != ConsoleKey.Enter)
            {
                return;
            }
            Guid id = Guid.NewGuid();

            PlaceOrder placeOrder = new PlaceOrder
            {
                Product = "New shoes",
                Id = id
            };
            bus.Send("Samples.StepByStep.Server", placeOrder);

            Console.WriteLine("Sent a new PlaceOrder message with id: {0}", id.ToString("N"));

        }

    }
    #endregion
}
