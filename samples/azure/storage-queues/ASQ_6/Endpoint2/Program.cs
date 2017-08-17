using System;
using NServiceBus;

class Program
{

    static void Main()
    {
        Console.Title = "Samples.Azure.StorageQueues.Endpoint2";
        var busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.Azure.StorageQueues.Endpoint2");
        busConfiguration.UseSerialization<JsonSerializer>();
        busConfiguration.EnableInstallers();
        busConfiguration.UseTransport<AzureStorageQueueTransport>()
            .ConnectionString("UseDevelopmentStorage=true");
        busConfiguration.UsePersistence<InMemoryPersistence>();

        using (var bus = Bus.Create(busConfiguration).Start())
        {
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}