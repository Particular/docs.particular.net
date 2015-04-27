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

        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.Logging.CommonLogging");

        #endregion

        busConfiguration.UseSerialization<JsonSerializer>();
        busConfiguration.EnableInstallers();
        busConfiguration.UsePersistence<InMemoryPersistence>();

        using (IStartableBus bus = Bus.Create(busConfiguration))
        {
            bus.Start();
            bus.SendLocal(new MyMessage());
            Console.WriteLine("\r\nPress any key to stop program\r\n");
            Console.ReadKey();
        }
    }
}
