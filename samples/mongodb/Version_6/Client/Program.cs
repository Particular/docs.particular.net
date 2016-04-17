using System;
using NServiceBus;

class Program
{

    static void Main()
    {
        Console.Title = "Samples.MongoDB.Client";
        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.MongoDB.Client");
        busConfiguration.UseSerialization<JsonSerializer>();
        busConfiguration.EnableInstallers();
        busConfiguration.UsePersistence<InMemoryPersistence>();

        using (IBus bus = Bus.Create(busConfiguration).Start())
        {
            Console.WriteLine("Press 'enter' to send a StartOrder messages");
            Console.WriteLine("Press any other key to exit");

            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey();
                Console.WriteLine();

                if (key.Key != ConsoleKey.Enter)
                {
                    return;
                }

                Guid orderId = Guid.NewGuid();
                bus.Send(new StartOrder
                          {
                              OrderId = orderId
                          });
                Console.WriteLine("StartOrder Message sent with OrderId  " + orderId);
            }
        }
    }
}