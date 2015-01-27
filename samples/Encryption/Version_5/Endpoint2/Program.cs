using System;
using NServiceBus;

class Program
{
    static void Main()
    {
        var busConfiguration = new BusConfiguration();
        busConfiguration.RijndaelEncryptionService("gdDbqRpqdRbTs3mhdZh9qCaDaxJXl+e6");
        busConfiguration.UsePersistence<InMemoryPersistence>();
        var startableBus = Bus.Create(busConfiguration);
        using (startableBus.Start())
        {
            Console.WriteLine("Press any key to exit");
            Console.ReadLine();
        }
    }
}