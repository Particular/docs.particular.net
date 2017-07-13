using System;
using NServiceBus;

class Program
{

    static void Main()
    {
        Console.Title = "Samples.DelayedDelivery.Server";
        var busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.DelayedDelivery.Server");
        busConfiguration.EnableInstallers();
        busConfiguration.UsePersistence<InMemoryPersistence>();

        using (var bus = Bus.Create(busConfiguration).Start())
        {
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}
