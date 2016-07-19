using log4net.Appender;
using log4net.Config;
using log4net.Core;
using log4net.Filter;
using log4net.Layout;
using NServiceBus.Log4Net;

public class Log4NetFiltering
{
    #region Log4NetFilter

    public class NServiceBusLogFilter :
        FilterSkeleton
    {
        public override FilterDecision Decide(LoggingEvent loggingEvent)
        {
            if (loggingEvent.LoggerName.StartsWith("NServiceBus."))
            {
                if (loggingEvent.Level < Level.Warn)
                {
                    return FilterDecision.Deny;
                }
            }
            return FilterDecision.Accept;
        }
    }

    #endregion
    public Log4NetFiltering()
    {
        #region Log4NetFilterUsage

        var appender = new ConsoleAppender
        {
            Threshold = Level.Debug,
            Layout = new SimpleLayout(),
        };

        appender.AddFilter(new NServiceBusLogFilter());
        appender.ActivateOptions();

        BasicConfigurator.Configure(appender);

        NServiceBus.Logging.LogManager.Use<Log4NetFactory>();

        #endregion
    }
}
