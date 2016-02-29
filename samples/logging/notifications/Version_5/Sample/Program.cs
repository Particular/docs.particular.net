using System;
using NServiceBus;
using NServiceBus.Logging;

class Program
{

    static void Main()
    {
        Console.Title = "Samples.Logging.Default";
        #region logging
        DefaultFactory defaultFactory = LogManager.Use<DefaultFactory>();
        defaultFactory.Level(LogLevel.Fatal);
        #endregion
        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.Logging.Default");
        busConfiguration.UseSerialization<JsonSerializer>();
        busConfiguration.EnableInstallers();
        busConfiguration.UsePersistence<InMemoryPersistence>();

        using (IBus bus = Bus.Create(busConfiguration).Start())
        {
            bus.SendLocal(new MyMessage());
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}