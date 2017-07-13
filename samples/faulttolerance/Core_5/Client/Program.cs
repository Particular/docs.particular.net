using System;
using NServiceBus;

class Program
{

    static void Main()
    {
        Console.Title = "Samples.FaultTolerance.Client";
        var busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.FaultTolerance.Client");
        busConfiguration.EnableInstallers();
        busConfiguration.UsePersistence<InMemoryPersistence>();

        using (var bus = Bus.Create(busConfiguration).Start())
        {
            Console.WriteLine("Press enter to send a message");
            Console.WriteLine("Press any key to exit");

            while (true)
            {
                var key = Console.ReadKey();
                if (key.Key != ConsoleKey.Enter)
                {
                    return;
                }
                var id = Guid.NewGuid();

                bus.Send("Samples.FaultTolerance.Server", new MyMessage
                {
                    Id = id
                });

                Console.WriteLine($"Sent a message with id: {id.ToString("N")}");
            }
        }
    }
}
