using System;
using NLog;
using NLog.Config;
using NLog.Targets;
using NServiceBus;

class Program
{

    static void Main()
    {

        #region ConfigureNLog

        LoggingConfiguration config = new LoggingConfiguration();

        ColoredConsoleTarget consoleTarget = new ColoredConsoleTarget
                                             {
                                                 Layout = "${level}|${logger}|${message}${onexception:${newline}${exception:format=tostring}}"
                                             };
        config.AddTarget("console", consoleTarget);
        config.LoggingRules.Add(new LoggingRule("*", LogLevel.Info, consoleTarget));

        LogManager.Configuration = config;
        #endregion
        #region UseConfig

        NServiceBus.Logging.LogManager.Use<NLogFactory>();

        // Then continue with your BusConfiguration
        BusConfiguration busConfig = new BusConfiguration();
        busConfig.EndpointName("Samples.Logging.NLogCustom");

        #endregion

        busConfig.UseSerialization<JsonSerializer>();
        busConfig.EnableInstallers();
        busConfig.UsePersistence<InMemoryPersistence>();

        using (IStartableBus bus = Bus.Create(busConfig))
        {
            bus.Start();
            bus.SendLocal(new MyMessage());
            Console.WriteLine("\r\nPress any key to stop program\r\n");
            Console.ReadKey();
        }
    }
}
