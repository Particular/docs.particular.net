using log4net.Appender;
using log4net.Config;
using log4net.Core;
using log4net.Filter;
using log4net.Layout;
using NServiceBus.Log4Net;

public class Log4NetConfig
{
    public void InCode()
    {
        #region Log4NetInCode

        var layout = new PatternLayout
        {
            ConversionPattern = "%d [%t] %-5p %c [%x] - %m%n"
        };
        layout.ActivateOptions();
        var consoleAppender = new ColoredConsoleAppender
        {
            Threshold = Level.Debug,
            Layout = layout
        };
        consoleAppender.ActivateOptions();
        var fileAppender = new RollingFileAppender
        {
            DatePattern = "yyyy-MM-dd'.txt'",
            RollingStyle = RollingFileAppender.RollingMode.Composite,
            MaxFileSize = 10 * 1024 * 1024,
            MaxSizeRollBackups = 10,
            LockingModel = new FileAppender.MinimalLock(),
            StaticLogFileName = false,
            File = @"nsb_log_",
            Layout = layout,
            AppendToFile = true,
            Threshold = Level.Debug,
        };
        fileAppender.ActivateOptions();

        BasicConfigurator.Configure(fileAppender, consoleAppender);

        NServiceBus.Logging.LogManager.Use<Log4NetFactory>();

        #endregion
    }
    public void Filtering()
    {
        #region Log4NetFiltering

        var consoleAppender = new ColoredConsoleAppender
        {
            Threshold = Level.Debug,
            Layout = new SimpleLayout(),
        };

        consoleAppender.AddFilter(new LoggerMatchFilter { LoggerToMatch = "MyNamespace" });
        consoleAppender.AddFilter(new DenyAllFilter());
        consoleAppender.ActivateOptions();

        BasicConfigurator.Configure(consoleAppender);

        NServiceBus.Logging.LogManager.Use<Log4NetFactory>();

        #endregion
    }
}