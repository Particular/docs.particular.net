using System;
using NServiceBus;
using NServiceBus.Persistence;
using NServiceBus.Persistence.Legacy;

class Program
{
    static void Main()
    {
        Console.Title = "Samples.Versioning.V1Subscriber";
        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.Versioning.V1Subscriber");
        busConfiguration.UseSerialization<JsonSerializer>();
        busConfiguration.UsePersistence<InMemoryPersistence>();
        busConfiguration.UsePersistence<MsmqPersistence, StorageType.Subscriptions>();
        busConfiguration.EnableInstallers();

        using (IBus bus = Bus.Create(busConfiguration).Start())
        {
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}