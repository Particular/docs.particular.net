using log4net.Appender;
using log4net.Config;
using log4net.Core;
using log4net.Layout;
using NServiceBus;

[EndpointName("Samples.Logging.HostCustom")]
#region Config
public class EndpointConfig : 
    IConfigureThisEndpoint, 
    AsA_Server,
    IWantCustomInitialization, 
    IWantCustomLogging
{
    void IWantCustomLogging.Init()
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

        SetLoggingLibrary.Log4Net();
    }
#endregion

    void IWantCustomInitialization.Init()
    {
        Configure configure = Configure.With();
        configure.DefaultBuilder();
        configure.InMemorySagaPersister();
        configure.RunTimeoutManagerWithInMemoryPersistence();
        configure.InMemorySubscriptionStorage();
        configure.JsonSerializer();
    }
}