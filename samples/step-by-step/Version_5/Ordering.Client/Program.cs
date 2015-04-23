using System;
using NServiceBus;

class Program
{
    #region ClientMain
    static void Main()
    {
        BusConfiguration busConfig = new BusConfiguration();
        busConfig.EndpointName("StepByStep.Ordering.Client");
        busConfig.UseSerialization<JsonSerializer>();
        busConfig.EnableInstallers();
        busConfig.UsePersistence<InMemoryPersistence>();

        using (IStartableBus startableBus = Bus.Create(busConfig))
        {
           var bus = startableBus.Start();
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
