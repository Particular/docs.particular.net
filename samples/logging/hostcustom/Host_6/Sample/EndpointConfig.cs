using log4net.Appender;
using log4net.Config;
using log4net.Core;
using log4net.Layout;
using NServiceBus.Log4Net;
using NServiceBus;
using NServiceBus.Logging;

[EndpointName("Samples.Logging.HostCustom")]

#region Config

public class EndpointConfig :
    IConfigureThisEndpoint,
    AsA_Server
{
    public EndpointConfig()
    {
        var layout = new PatternLayout
        {
            ConversionPattern = "%d %-5p %c - %m%n"
        };
        layout.ActivateOptions();
        var appender = new ConsoleAppender
        {
            Layout = layout,
            Threshold = Level.Info
        };
        appender.ActivateOptions();

        BasicConfigurator.Configure(appender);

        LogManager.Use<Log4NetFactory>();
    }

    #endregion

    public void Customize(BusConfiguration busConfiguration)
    {
        busConfiguration.EnableInstallers();
        busConfiguration.UsePersistence<InMemoryPersistence>();
    }
}