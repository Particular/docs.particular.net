using System;
using NServiceBus;

static class Program
{
    static void Main()
    {
        BusConfiguration configuration = new BusConfiguration();
        configuration.EndpointName("Samples.Mvc.Endpoint");
        configuration.UseSerialization<JsonSerializer>();
        configuration.UsePersistence<InMemoryPersistence>();
        configuration.EnableInstallers();

        using (IStartableBus bus = Bus.Create(configuration))
        {
            bus.Start();
            Console.WriteLine("To exit, press Ctrl + C");
            Console.ReadLine();
        }
    }
}