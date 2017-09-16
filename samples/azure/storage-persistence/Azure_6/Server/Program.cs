using System;
using NServiceBus;

class Program
{
    static void Main()
    {
        Console.Title = "Samples.Azure.StoragePersistence.Server";
        var busConfiguration = new BusConfiguration();
        
        #region config

        busConfiguration.EndpointName("Samples.Azure.StoragePersistence.Server");
        busConfiguration.UsePersistence<AzureStoragePersistence>();

        #endregion

        busConfiguration.UseTransport<AzureStorageQueueTransport>()
            .ConnectionString("UseDevelopmentStorage=true");
        busConfiguration.UseSerialization<JsonSerializer>();
        busConfiguration.EnableInstallers();

        using (var bus = Bus.Create(busConfiguration).Start())
        {
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}