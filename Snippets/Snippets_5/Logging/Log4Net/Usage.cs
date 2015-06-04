namespace Snippets5.Logging.Log4Net
{
    using log4net.Appender;
    using log4net.Config;
    using log4net.Core;
    using log4net.Layout;
    using NServiceBus.Log4Net;
    using NServiceBus.Logging;

    public class Usage
    {
        public Usage()
        {
            #region Log4NetInCode

            PatternLayout layout = new PatternLayout
            {
                ConversionPattern = "%d [%t] %-5p %c [%x] - %m%n"
            };
            layout.ActivateOptions();
            ColoredConsoleAppender consoleAppender = new ColoredConsoleAppender
            {
                Threshold = Level.Debug,
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
                File = @"nsb_log_",
                Layout = layout,
                AppendToFile = true,
                Threshold = Level.Debug,
            };
            fileAppender.ActivateOptions();

            BasicConfigurator.Configure(fileAppender, consoleAppender);

            LogManager.Use<Log4NetFactory>();

            #endregion
        }
    }
}