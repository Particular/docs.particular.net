using log4net.Appender;
using log4net.Config;
using log4net.Core;
using log4net.Layout;
using NServiceBus.Log4Net;
using NServiceBus;
using NServiceBus.Logging;

#region Config
public class EndpointConfig : 
    IConfigureThisEndpoint, 
    AsA_Server
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
#endregion
    public void Customize(BusConfiguration configuration)
    {
        configuration.EndpointName("Samples.Logging.HostCustom");
        configuration.UseSerialization<JsonSerializer>();
        configuration.EnableInstallers();
        configuration.UsePersistence<InMemoryPersistence>();
    }
}