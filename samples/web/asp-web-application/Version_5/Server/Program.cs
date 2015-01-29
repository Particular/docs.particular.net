using System;
using NServiceBus;

class Program
{
    static void Main()
    {
        var busConfig = new BusConfiguration();
        busConfig.EndpointName("Samples.AsyncPages.Server");
        busConfig.UseSerialization<JsonSerializer>();
        busConfig.EnableInstallers();
        busConfig.UsePersistence<InMemoryPersistence>();
        using (var bus = Bus.Create(busConfig))
        {
            bus.Start();
            Console.WriteLine("\r\nPress any key to stop program\r\n");
            Console.ReadKey();
        }

        Console.WriteLine("\r\nPress any key to stop program\r\n");
        Console.ReadKey();
    }
}

