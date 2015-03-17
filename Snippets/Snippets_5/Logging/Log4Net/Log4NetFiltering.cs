using log4net.Appender;
using log4net.Config;
using log4net.Core;
using log4net.Filter;
using log4net.Layout;
using NServiceBus.Log4Net;

public class Log4NetFiltering
{
    public void Filtering()
    {
        #region Log4NetFiltering

        ColoredConsoleAppender appender = new ColoredConsoleAppender
        {
            Threshold = Level.Debug,
            Layout = new SimpleLayout(),
        };

        appender.AddFilter(new LoggerMatchFilter
                           {
                               LoggerToMatch = "MyNamespace"
                           });
        appender.AddFilter(new DenyAllFilter());
        appender.ActivateOptions();

        BasicConfigurator.Configure(appender);

        NServiceBus.Logging.LogManager.Use<Log4NetFactory>();

        #endregion
    }
}