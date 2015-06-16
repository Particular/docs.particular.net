using System;
using log4net.Appender;
using log4net.Config;
using log4net.Core;
using log4net.Layout;
using NServiceBus;
using NServiceBus.Log4Net;
using NServiceBus.Logging;

class Program
{

    static void Main()
    {

        #region ConfigureLog4Net
        PatternLayout layout = new PatternLayout
        {
            ConversionPattern = "%d [%t] %-5p %c [%x] - %m%n"
        };
        layout.ActivateOptions();
        ColoredConsoleAppender consoleAppender = new ColoredConsoleAppender
        {
            Threshold = Level.Info,
            Layout = layout
        };
        consoleAppender.ActivateOptions();
        RollingFileAppender fileAppender = new RollingFileAppender
        {
            DatePattern = "yyyy-MM-dd'.txt'",
            RollingStyle = RollingFileAppender.RollingMode.Composite,
            MaxFileSize = 10 * 1024 * 1024,
            MaxSizeRollBackups = 10,
            LockingModel = new FileAppender.MinimalLock(),
            StaticLogFileName = false,
            File = @"nsblog",
            Layout = layout,
            AppendToFile = true,
            Threshold = Level.Debug,
        };
        fileAppender.ActivateOptions();

        BasicConfigurator.Configure(fileAppender, consoleAppender);
        #endregion
        #region UseConfig

        LogManager.Use<Log4NetFactory>();

        // Then continue with your BusConfiguration
        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.Logging.Log4NetCustom");

        #endregion

        busConfiguration.UseSerialization<JsonSerializer>();
        busConfiguration.EnableInstallers();
        busConfiguration.UsePersistence<InMemoryPersistence>();

        using (IBus bus = Bus.Create(busConfiguration).Start())
        {
            bus.SendLocal(new MyMessage());
            Console.WriteLine("\r\nPress any key to stop program\r\n");
            Console.ReadKey();
        }
    }
}
