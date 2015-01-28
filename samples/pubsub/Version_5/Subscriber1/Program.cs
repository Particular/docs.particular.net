using System;
using NServiceBus;

static class Program
{

    static void Main()
    {
        var busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Sample.PubSub.Subscriber1");
        busConfiguration.UseSerialization<JsonSerializer>();
        busConfiguration.UsePersistence<InMemoryPersistence>();
        busConfiguration.EnableInstallers();
        var startableBus = Bus.Create(busConfiguration);
        using (startableBus.Start())
        {
            Console.WriteLine("To exit, Ctrl + C");
            Console.ReadLine();
        }
    }

}