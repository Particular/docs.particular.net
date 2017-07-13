using System;
using NServiceBus;

class Program
{
    static void Main()
    {
        Console.Title = "Samples.AsyncPages.Server";
        var busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.AsyncPages.Server");
        busConfiguration.EnableInstallers();
        busConfiguration.UsePersistence<InMemoryPersistence>();
        using (var bus = Bus.Create(busConfiguration).Start())
        {
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}

