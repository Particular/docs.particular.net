namespace HostDefaultLogging_4
{
    using NServiceBus;
    
    public class EndpointConfig : IConfigureThisEndpoint, AsA_Server, IWantCustomInitialization
    {
        public void Init()
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
