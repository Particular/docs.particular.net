using System;
using NServiceBus;

class Program
{

    static void Main()
    {
        Console.Title = "Samples.Store.Sales";
        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Store.Sales");
        busConfiguration.ApplyCommonConfiguration();
        using (IBus bus = Bus.Create(busConfiguration).Start())
        {
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}
