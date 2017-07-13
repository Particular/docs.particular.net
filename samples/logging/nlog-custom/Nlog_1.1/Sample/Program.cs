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

        var config = new LoggingConfiguration();

        var consoleTarget = new ColoredConsoleTarget
        {
            Layout = "${level}|${logger}|${message}${onexception:${newline}${exception:format=tostring}}"
        };
        config.AddTarget("console", consoleTarget);
        config.LoggingRules.Add(new LoggingRule("*", LogLevel.Info, consoleTarget));

        LogManager.Configuration = config;

        #endregion

        #region UseConfig

        NServiceBus.Logging.LogManager.Use<NLogFactory>();

        var busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.Logging.NLogCustom");

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