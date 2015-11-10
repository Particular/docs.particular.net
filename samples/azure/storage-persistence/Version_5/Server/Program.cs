using System;
using NServiceBus;

class Program
{

    static void Main()
    {
        #region config

        BusConfiguration busConfiguration = new BusConfiguration();
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