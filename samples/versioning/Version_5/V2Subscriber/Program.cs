using System;
using NServiceBus;
using NServiceBus.Persistence;
using NServiceBus.Persistence.Legacy;

class Program
{
    static void Main()
    {
        var busConfig = new BusConfiguration();
        busConfig.EndpointName("Samples.Versioning.V2Subscriber");
        busConfig.UseSerialization<JsonSerializer>();
        busConfig.UsePersistence<InMemoryPersistence>();
        busConfig.UsePersistence<MsmqPersistence>()
            .For(Storage.Subscriptions);
        busConfig.EnableInstallers();

        using (var bus = Bus.Create(busConfig))
        {
            bus.Start();
            Console.WriteLine("\r\nPress any key to stop program\r\n");
            Console.ReadKey();
        }
    }
}