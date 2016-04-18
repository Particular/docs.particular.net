namespace Log4Net_1_1
{
    using log4net.Appender;
    using log4net.Config;
    using log4net.Core;
    using log4net.Layout;
    using NServiceBus.Log4Net;
    using NServiceBus.Logging;

    class Usage
    {
        Usage()
        {
            #region Log4NetInCode

            PatternLayout layout = new PatternLayout
            {
                ConversionPattern = "%d [%t] %-5p %c [%x] - %m%n"
            };
            layout.ActivateOptions();
            ConsoleAppender consoleAppender = new ConsoleAppender
            {
                Threshold = Level.Debug,
                Layout = layout
            };
            // Note that ActivateOptions is required in NSB 5 and above
            consoleAppender.ActivateOptions();

            BasicConfigurator.Configure(consoleAppender);

            LogManager.Use<Log4NetFactory>();

            #endregion
        }
    }
}