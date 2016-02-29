using System;
using NServiceBus;

class Program
{

    static void Main()
    {
        Console.Title = "Samples.MessageDurability.Receiver";
        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.Transactions()
            .Disable();
        busConfiguration.EndpointName("Samples.MessageDurability.Receiver");
        busConfiguration.UseSerialization<JsonSerializer>();
        busConfiguration.EnableInstallers();
        busConfiguration.UsePersistence<InMemoryPersistence>();

        using (IBus bus = Bus.Create(busConfiguration).Start())
        {
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}
