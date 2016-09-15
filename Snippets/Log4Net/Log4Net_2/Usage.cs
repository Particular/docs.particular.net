using log4net.Appender;
using log4net.Config;
using log4net.Core;
using log4net.Layout;
using NServiceBus;
using NServiceBus.Logging;

class Usage
{
    Usage()
    {
        #region Log4NetInCode

        var layout = new PatternLayout
        {
            ConversionPattern = "%d [%t] %-5p %c [%x] - %m%n"
        };
        layout.ActivateOptions();
        var consoleAppender = new ConsoleAppender
        {
            Threshold = Level.Debug,
            Layout = layout
        };
        consoleAppender.ActivateOptions();

        BasicConfigurator.Configure(consoleAppender);

        LogManager.Use<Log4NetFactory>();

        #endregion
    }
}
