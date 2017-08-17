using System;
using NServiceBus;

class Program
{

    static void Main()
    {
        Console.Title = "Samples.Azure.StorageQueues.Endpoint2";
        var busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.Azure.StorageQueues.Endpoint2").UseConnectionString("UseDevelopmentStorage=true");
        busConfiguration.UseSerialization<JsonSerializer>();
        busConfiguration.EnableInstallers();
        busConfiguration.UseTransport<AzureStorageQueueTransport>();
        busConfiguration.UsePersistence<InMemoryPersistence>();

        using (var bus = Bus.Create(busConfiguration).Start())
        {
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}