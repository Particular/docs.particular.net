using System;
using NServiceBus;
using NServiceBus.Logging;

static class Program
{
    static void Main()
    {
        Console.Title = "Samples.PubSub.Subscriber";
        LogManager.Use<DefaultFactory>().Level(LogLevel.Info);
        var busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.PubSub.Subscriber");
        busConfiguration.UseSerialization<JsonSerializer>();
        busConfiguration.UsePersistence<InMemoryPersistence>();
        busConfiguration.EnableInstallers();
        using (Bus.Create(busConfiguration).Start())
        {
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}