using log4net.Appender;
using log4net.Config;
using log4net.Core;
using log4net.Layout;
using NServiceBus;

static class LoggingHelper
{
    public static void ConfigureLogging(Level threshold)
    {
        var layout = new PatternLayout
                     {
                         ConversionPattern = "%d [%t] %-5p %c [%x] - %m%n"
                     };
        layout.ActivateOptions();
        var appender = new ConsoleAppender
                       {
                           Layout = layout,
                           Threshold = threshold
                       };
        appender.ActivateOptions();

        BasicConfigurator.Configure(appender);

        SetLoggingLibrary.Log4Net();
    }
}