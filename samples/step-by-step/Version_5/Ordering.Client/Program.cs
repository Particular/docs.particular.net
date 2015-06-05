using System;
using NServiceBus;

class Program
{
    #region ClientMain
    static void Main()
    {
        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("StepByStep.Ordering.Client");
        busConfiguration.UseSerialization<JsonSerializer>();
        busConfiguration.EnableInstallers();
        busConfiguration.UsePersistence<InMemoryPersistence>();

        using (IBus bus = Bus.Create(busConfiguration).Start())
        {
           SendOrder(bus);
        }
    }
    #endregion

    #region SendOrder
    static void SendOrder(IBus bus)
    {
        Console.WriteLine("Press 'Enter' to send a message. To exit press 'Ctrl + C'");

        while (Console.ReadLine() != null)
        {
            Guid id = Guid.NewGuid();

            PlaceOrder placeOrder = new PlaceOrder
            {
                Product = "New shoes",
                Id = id
            };
            bus.Send("StepByStep.Ordering.Server", placeOrder);

            Console.WriteLine("Sent a new PlaceOrder message with id: {0}", id.ToString("N"));
        }
    }
    #endregion
}
