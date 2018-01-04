using System;
using NServiceBus;

static class Program
{
    static void Main()
    {
        Console.Title = "Samples.Nancy.Endpoint";
        var busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.Nancy.Endpoint");
        busConfiguration.UsePersistence<InMemoryPersistence>();
        busConfiguration.EnableInstallers();

        using (var bus = Bus.Create(busConfiguration).Start())
        {
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}