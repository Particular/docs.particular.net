using System;
using NServiceBus;
using Store.Shared;


class Program
{

    static void Main()
    {
        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Store.Operations");
        busConfiguration.ApplyCommonConfiguration();
        using (IBus bus = Bus.Create(busConfiguration).Start())
        {
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}
