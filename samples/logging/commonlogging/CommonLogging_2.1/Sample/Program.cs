using System;
using Common.Logging;
using NServiceBus;
using Common.Logging.Simple;
// ReSharper disable RedundantNameQualifier

class Program
{

    static void Main()
    {
        Console.Title = "Samples.Logging.CommonLogging";

        #region ConfigureLogging

        Common.Logging.LogManager.Adapter = new ConsoleOutLoggerFactoryAdapter
        {
            Level = LogLevel.Info
        };

        NServiceBus.Logging.LogManager.Use<CommonLoggingFactory>();

        var busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.Logging.CommonLogging");

        #endregion

        busConfiguration.UseSerialization<JsonSerializer>();
        busConfiguration.EnableInstallers();
        busConfiguration.UsePersistence<InMemoryPersistence>();

        using (var bus = Bus.Create(busConfiguration).Start())
        {
            bus.SendLocal(new MyMessage());
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}