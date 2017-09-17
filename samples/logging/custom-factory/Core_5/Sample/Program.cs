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

        // optionally set the log level in code or read from app.config
        loggerDefinition.Level(LogLevel.Info);

        // logging configuration should occur prior to endpoint configuration
        var busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.Logging.CustomFactory");

        #endregion
        busConfiguration.EnableInstallers();
        busConfiguration.UsePersistence<InMemoryPersistence>();

        using (var bus = Bus.Create(busConfiguration).Start())
        {
            var myMessage = new MyMessage();
            bus.SendLocal(myMessage);
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}