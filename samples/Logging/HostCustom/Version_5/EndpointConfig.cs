using log4net.Appender;
using log4net.Config;
using log4net.Layout;
using NServiceBus.Log4Net;
using NServiceBus;
using NServiceBus.Logging;

namespace HostCustomLogging_5
{
    public class EndpointConfig : IConfigureThisEndpoint, AsA_Server
    {
        public EndpointConfig()
        {
            var layout = new PatternLayout
                         {
                             ConversionPattern = "%d [%t] %-5p %c [%x] - %m%n"
                         };
            layout.ActivateOptions();
            var appender = new ConsoleAppender
                           {
                               Layout = layout
                           };
            appender.ActivateOptions();

            BasicConfigurator.Configure(appender);

            LogManager.Use<Log4NetFactory>();
        }

        public void Customize(BusConfiguration configuration)
        {
            configuration.EndpointName("HostCustomLoggingSample");
            configuration.UseSerialization<JsonSerializer>();
            configuration.EnableInstallers();
            configuration.UsePersistence<InMemoryPersistence>();
        }
    }
}
