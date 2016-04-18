using System;
using NServiceBus;

class Program
{

    static void Main()
    {
        Console.Title = "Samples.FaultTolerance.Client";
        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.FaultTolerance.Client");
        busConfiguration.UseSerialization<JsonSerializer>();
        busConfiguration.EnableInstallers();
        busConfiguration.UsePersistence<InMemoryPersistence>();

        using (IBus bus = Bus.Create(busConfiguration).Start())
        {
            Console.WriteLine("Press enter to send a message");
            Console.WriteLine("Press any key to exit");

            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey();
                if (key.Key != ConsoleKey.Enter)
                {
                    return;
                }
                Guid id = Guid.NewGuid();

                bus.Send("Samples.FaultTolerance.Server", new MyMessage
                {
                    Id = id
                });

                Console.WriteLine("Sent a new message with id: {0}", id.ToString("N"));
            }
        }
    }
}
