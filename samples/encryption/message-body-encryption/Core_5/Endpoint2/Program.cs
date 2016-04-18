using System;
using NServiceBus;

class Program
{
    static void Main()
    {
        Console.Title = "Samples.MessageBodyEncryption.Endpoint2";
        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.MessageBodyEncryption.Endpoint2");
        busConfiguration.UsePersistence<InMemoryPersistence>();
        busConfiguration.RegisterMessageEncryptor();
        IStartableBus startableBus = Bus.Create(busConfiguration);
        using (startableBus.Start())
        {
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}