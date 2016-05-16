using System;
using NServiceBus;

class Program
{
    #region StepByStep-ClientInit
    static void Main()
    {
        // This makes it easier to tell console windows apart.
        Console.Title = "Samples.StepByStep.Client";

        BusConfiguration busConfiguration = new BusConfiguration();

        // The endpoint name will be used to determine queue names and serves
        // as the address, or identity, of the endpoint.
        busConfiguration.EndpointName("Samples.StepByStep.Client");

        // Use JSON to serialize and deserialize messages (which are just
        // plain classes) to and from message queues.
        busConfiguration.UseSerialization<JsonSerializer>();

        // Ask NServiceBus to automatically create message queues for us.
        busConfiguration.EnableInstallers();

        // Store information in memory for this example, rather than in
        // a database. In this sample, we will only end up storing
        // subscription information.
        busConfiguration.UsePersistence<InMemoryPersistence>();

        // Initialize the endpoint with the configuration we've built
        using (IBus bus = Bus.Create(busConfiguration).Start())
        {
           SendOrder(bus);
        }
    }
    #endregion


    #region StepByStep-SendOrder
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
