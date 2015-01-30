using System;
using NServiceBus;

static class Program
{
    static void Main()
    {
        var configuration = new BusConfiguration();
        configuration.EndpointName("Samples.Mvc.Endpoint");
        configuration.UseSerialization<JsonSerializer>();
        configuration.UsePersistence<InMemoryPersistence>();
        configuration.EnableInstallers();

        using (var bus = Bus.Create(configuration))
        {
            bus.Start();
            Console.WriteLine("To exit, press Ctrl + C");
            Console.ReadLine();
        }
    }
}