using log4net.Appender;
using log4net.Config;
using log4net.Core;
using log4net.Layout;
using NServiceBus;
using NServiceBus.Log4Net;
using NServiceBus.Logging;

#region nservicebus-host

public class EndpointConfig : IConfigureThisEndpoint
{
    public EndpointConfig()
    {
        PatternLayout layout = new PatternLayout
        {
            ConversionPattern = "%d %-5p %c - %m%n"
        };
        layout.ActivateOptions();
        ConsoleAppender appender = new ConsoleAppender
        {
            Layout = layout,
            Threshold = Level.Info
        };
        appender.ActivateOptions();

        BasicConfigurator.Configure(appender);

        LogManager.Use<Log4NetFactory>();
    }

    public void Customize(BusConfiguration busConfiguration)
    {
        busConfiguration.EndpointName("Samples.NServiceBus.HostCustom");
        busConfiguration.UseSerialization<JsonSerializer>();
        busConfiguration.EnableInstallers();
        busConfiguration.SendFailedMessagesTo("error");
        busConfiguration.UsePersistence<InMemoryPersistence>();
    }
}

#endregion