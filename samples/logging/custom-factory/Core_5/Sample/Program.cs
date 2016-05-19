using System;
using NServiceBus;
using NServiceBus.Logging;

class Program
{

    static void Main()
    {
        Console.Title = "Samples.Logging.CustomFactory";
        #region ConfigureLogging

        var loggerDefinition = LogManager.Use<ConsoleLoggerDefinition>();
        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.Logging.CustomFactory");

        #endregion
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