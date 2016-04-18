using System;
using NServiceBus;

class Program
{

    static void Main()
    {
        Console.Title = "Samples.Store.ContentManagement";
        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Store.ContentManagement");
        busConfiguration.ApplyCommonConfiguration();
        using (IBus bus = Bus.Create(busConfiguration).Start())
        {
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}
