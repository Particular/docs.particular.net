using System;
using NServiceBus;

static class Program
{
    static void Main()
    {
        Console.Title = "Samples.Mvc.Endpoint";
        var busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.Mvc.Endpoint");
        busConfiguration.UsePersistence<InMemoryPersistence>();
        busConfiguration.EnableInstallers();

        using (var bus = Bus.Create(busConfiguration).Start())
        {
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}