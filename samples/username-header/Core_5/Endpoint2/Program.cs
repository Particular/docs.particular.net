using System;
using NServiceBus;

class Program
{
    static void Main()
    {
        Console.Title = "Samples.UsernameHeader.Endpoint2";
        var busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.UsernameHeader.Endpoint2");
        busConfiguration.UsePersistence<InMemoryPersistence>();

        using (Bus.Create(busConfiguration).Start())
        {
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}