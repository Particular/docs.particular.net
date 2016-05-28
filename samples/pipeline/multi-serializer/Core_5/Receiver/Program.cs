using System;
using NServiceBus;

class Program
{

    static void Main()
    {
        Console.Title = "Samples.MultiSerializer.Receiver";
        var busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.MultiSerializer.Receiver");
        busConfiguration.UsePersistence<InMemoryPersistence>();
        busConfiguration.EnableInstallers();
        using (var bus = Bus.Create(busConfiguration).Start())
        {
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}