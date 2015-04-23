using System;
using NServiceBus;

static class Program
{
    static void Main()
    {
        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.Mvc.Endpoint");
        busConfiguration.UseSerialization<JsonSerializer>();
        busConfiguration.UsePersistence<InMemoryPersistence>();
        busConfiguration.EnableInstallers();

        using (IStartableBus bus = Bus.Create(busConfiguration))
        {
            bus.Start();
            Console.WriteLine("To exit, press Ctrl + C");
            Console.ReadLine();
        }
    }
}