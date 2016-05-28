using System;
using NServiceBus;

class Program
{

    static void Main()
    {
        Console.Title = "Samples.Store.Operations";
        var busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Store.Operations");
        busConfiguration.ApplyCommonConfiguration();
        using (var bus = Bus.Create(busConfiguration).Start())
        {
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}
