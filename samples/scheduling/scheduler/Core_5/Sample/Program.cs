using System;
using NServiceBus;

class Program
{
    static void Main()
    {
        Console.Title = "Samples.Scheduling";
        var busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.Scheduling");
        busConfiguration.EnableInstallers();
        busConfiguration.UsePersistence<InMemoryPersistence>();

        using (Bus.Create(busConfiguration).Start())
        {
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}