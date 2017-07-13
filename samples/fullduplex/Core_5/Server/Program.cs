using System;
using NServiceBus;
using NServiceBus.Logging;

class Program
{

    static void Main()
    {
        Console.Title = "Samples.FullDuplex.Server";
        LogManager.Use<DefaultFactory>()
            .Level(LogLevel.Info);
        var busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.FullDuplex.Server");
        busConfiguration.UsePersistence<InMemoryPersistence>();
        busConfiguration.EnableInstallers();

        using (var bus = Bus.Create(busConfiguration).Start())
        {
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}