using System;
using NServiceBus;
using NServiceBus.Features;
using NServiceBus.Logging;

static class Program
{
    static void Main()
    {
        Console.Title = "Samples.PubSub.Subscriber2";
        LogManager.Use<DefaultFactory>().Level(LogLevel.Info);
        var busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.PubSub.Subscriber2");
        busConfiguration.UseSerialization<JsonSerializer>();
        busConfiguration.DisableFeature<AutoSubscribe>();
        busConfiguration.UsePersistence<InMemoryPersistence>();
        busConfiguration.EnableInstallers();
        using (var bus = Bus.Create(busConfiguration).Start())
        {
            bus.Subscribe<IMyEvent>();
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
            bus.Unsubscribe<IMyEvent>();
        }
    }
}