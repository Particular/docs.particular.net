using System;
using NServiceBus;

class Program
{
    static void Main()
    {
        Console.Title = "Samples.Encryption.Endpoint2";
        var busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.Encryption.Endpoint2");
        busConfiguration.ConfigurationEncryption();
        busConfiguration.UsePersistence<InMemoryPersistence>();
        using (Bus.Create(busConfiguration).Start())
        {
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}