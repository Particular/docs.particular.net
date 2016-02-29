using System;
using NServiceBus;

class Program
{

    static void Main()
    {
        Console.Title = "Samples.Store.CustomerRelations";
        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Store.CustomerRelations");
        busConfiguration.ApplyCommonConfiguration();
        using (IBus bus = Bus.Create(busConfiguration).Start())
        {
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}
