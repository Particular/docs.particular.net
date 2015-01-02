using log4net.Appender;
using log4net.Config;
using log4net.Core;
using log4net.Filter;
using log4net.Layout;
using NServiceBus;

public class Log4NetFiltering
{

    public void Filtering()
    {
        #region Log4NetFiltering

        var appender = new ColoredConsoleAppender
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

        SetLoggingLibrary.Log4Net();

        #endregion
    }
}