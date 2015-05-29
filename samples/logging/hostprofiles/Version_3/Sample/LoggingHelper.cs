using log4net.Appender;
using log4net.Config;
using log4net.Core;
using log4net.Layout;
using NServiceBus;

#region LoggingHelper
static class LoggingHelper
{
    public static void ConfigureLogging(Level threshold)
    {
        PatternLayout layout = new PatternLayout
                     {
                         ConversionPattern = "%d %-5p %c - %m%n"
                     };
        layout.ActivateOptions();
        ConsoleAppender appender = new ConsoleAppender
                       {
                           Layout = layout,
                           Threshold = threshold
                       };
        appender.ActivateOptions();

        BasicConfigurator.Configure(appender);

        SetLoggingLibrary.Log4Net();
    }
}
#endregion