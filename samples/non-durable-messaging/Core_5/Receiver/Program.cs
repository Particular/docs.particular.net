using System;
using NServiceBus;

class Program
{
    static void Main()
    {
        Console.Title = "Samples.MessageDurability.Receiver";
        var busConfiguration = new BusConfiguration();
        busConfiguration.Transactions()
            .Disable();
        busConfiguration.EndpointName("Samples.MessageDurability.Receiver");
        busConfiguration.EnableInstallers();
        busConfiguration.UsePersistence<InMemoryPersistence>();

        using (var bus = Bus.Create(busConfiguration).Start())
        {
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}
