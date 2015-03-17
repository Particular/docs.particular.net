using System;
using NServiceBus;
using NServiceBus.Logging;

static class Program
{

    static void Main()
    {
        LogManager.Use<DefaultFactory>().Level(LogLevel.Info);
        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.PubSub.Subscriber1");
        busConfiguration.UseSerialization<JsonSerializer>();
        busConfiguration.UsePersistence<InMemoryPersistence>();
        busConfiguration.EnableInstallers();
        IStartableBus startableBus = Bus.Create(busConfiguration);
        using (startableBus.Start())
        {
            Console.WriteLine("To exit, Ctrl + C");
            Console.ReadLine();
        }
    }

}