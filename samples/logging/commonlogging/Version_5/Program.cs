using System;
using Common.Logging;
using NServiceBus;
using Common.Logging.Simple;
// ReSharper disable RedundantNameQualifier

class Program
{

    static void Main()
    {
        #region ConfigureLogging

        Common.Logging.LogManager.Adapter = new ConsoleOutLoggerFactoryAdapter
                                            {
                                                Level = LogLevel.Info
                                            };

        NServiceBus.Logging.LogManager.Use<CommonLoggingFactory>();

        var busConfig = new BusConfiguration();
        busConfig.EndpointName("Samples.Logging.CommonLogging");

        #endregion

        busConfig.UseSerialization<JsonSerializer>();
        busConfig.EnableInstallers();
        busConfig.UsePersistence<InMemoryPersistence>();

        using (var bus = Bus.Create(busConfig))
        {
            bus.Start();
            bus.SendLocal(new MyMessage());
            Console.WriteLine("\r\nPress any key to stop program\r\n");
            Console.ReadKey();
        }
    }
}
