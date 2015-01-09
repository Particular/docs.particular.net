using NServiceBus;

namespace HostDefaultLogging_3_3
{

    public class EndpointConfig : IConfigureThisEndpoint, AsA_Server,IWantCustomInitialization
    {
        public void Init()
        {
            var configure = Configure.With();
            configure.DefaultBuilder();
            configure.InMemorySagaPersister();
            configure.UseInMemoryTimeoutPersister();
            configure.InMemorySubscriptionStorage();
            configure.JsonSerializer();
        }
    }

}