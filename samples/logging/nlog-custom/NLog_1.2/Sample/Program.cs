using System;
using NLog;
using NLog.Config;
using NLog.Targets;
using NServiceBus;

class Program
{

    static void Main()
    {
        Console.Title = "Samples.Logging.NLogCustom";

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

        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.Logging.NLogCustom");

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
