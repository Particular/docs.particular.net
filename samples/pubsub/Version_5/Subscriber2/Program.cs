using System;
using MyMessages;
using NServiceBus;
using NServiceBus.Features;

static class Program
{
    static void Main()
    {
        var busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Sample.PubSub.Subscriber2");
        busConfiguration.UseSerialization<JsonSerializer>();
        busConfiguration.DisableFeature<AutoSubscribe>();
        busConfiguration.UsePersistence<InMemoryPersistence>();
        busConfiguration.EnableInstallers();
        var startableBus = Bus.Create(busConfiguration);
        using (var bus = startableBus.Start())
        {
            bus.Subscribe<IMyEvent>();
            Console.WriteLine("To exit, Ctrl + C");
            Console.ReadLine();
            bus.Unsubscribe<IMyEvent>();
        }
    }
}