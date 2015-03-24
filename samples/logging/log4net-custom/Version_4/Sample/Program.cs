using System;
using log4net.Appender;
using log4net.Config;
using log4net.Core;
using log4net.Layout;
using NServiceBus;
using NServiceBus.Installation.Environments;

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
        // Note that no consoleAppender.ActivateOptions(); is required since NSB 4 does this internally

        RollingFileAppender fileAppender = new RollingFileAppender
        {
            DatePattern = "yyyy-MM-dd'.txt'",
            RollingStyle = RollingFileAppender.RollingMode.Composite,
            MaxFileSize = 10 * 1024 * 1024,
            MaxSizeRollBackups = 10,
            LockingModel = new FileAppender.MinimalLock(),
            StaticLogFileName = false,
            File = @"nsblog2",
            Layout = layout,
            AppendToFile = true,
            Threshold = Level.Info,
        };
        // Note that no fileAppender.ActivateOptions(); is required since NSB 4 does this internally
        BasicConfigurator.Configure(fileAppender, consoleAppender);
        #endregion

        Configure.Serialization.Json();

        #region UseConfig
        Configure configure = Configure.With();
        configure.DefineEndpointName("Samples.Logging.Log4NetCustom");

        //Pass the appenders to NServiceBus
        configure.Log4Net(consoleAppender);
        configure.Log4Net(fileAppender);
        #endregion
        configure.DefaultBuilder();
        configure.InMemorySagaPersister();
        configure.UseInMemoryTimeoutPersister();
        configure.InMemorySubscriptionStorage();
        configure.UseTransport<Msmq>();
        IBus bus = configure.UnicastBus()
            .CreateBus()
            .Start(() => Configure.Instance.ForInstallationOn<Windows>().Install());

        bus.SendLocal(new MyMessage());

        Console.WriteLine("\r\nPress any key to stop program\r\n");
        Console.ReadKey();
    }
}