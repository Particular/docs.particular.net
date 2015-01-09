
using log4net.Appender;
using log4net.Config;
using log4net.Layout;

namespace HostCustomLogging_4
{
    using NServiceBus;

    public class EndpointConfig : IConfigureThisEndpoint, AsA_Server, IWantCustomInitialization, IWantCustomLogging
    {
        void IWantCustomLogging.Init()
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

            SetLoggingLibrary.Log4Net();
        }

        void IWantCustomInitialization.Init()
        {
            Configure.Serialization.Json();
            var configure = Configure.With();
            configure.DefaultBuilder();
            configure.InMemorySagaPersister();
            configure.UseInMemoryTimeoutPersister();
            configure.InMemorySubscriptionStorage();
        }
    }
}
