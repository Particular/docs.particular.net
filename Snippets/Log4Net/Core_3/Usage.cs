using log4net.Appender;
using log4net.Core;
using log4net.Layout;
using NServiceBus;

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
        // Note that no ActivateOptions is required since NSB 3 does this internally

        // Pass the appender to NServiceBus
        SetLoggingLibrary.Log4Net(null, consoleAppender);

        #endregion
    }
}
