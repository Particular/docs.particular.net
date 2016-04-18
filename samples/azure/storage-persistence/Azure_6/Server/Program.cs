using System;
using NServiceBus;

class Program
{

    static void Main()
    {
        Console.Title = "Samples.Azure.StoragePersistence.Server";
        BusConfiguration busConfiguration = new BusConfiguration();
        #region config

        busConfiguration.EndpointName("Samples.Azure.StoragePersistence.Server");
        busConfiguration.UsePersistence<AzureStoragePersistence>();

        #endregion
        busConfiguration.UseSerialization<JsonSerializer>();
        busConfiguration.EnableInstallers();

        using (IBus bus = Bus.Create(busConfiguration).Start())
        {
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}