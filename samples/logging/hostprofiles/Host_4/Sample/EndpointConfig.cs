using NServiceBus;

[EndpointName("Samples.Logging.HostProfiles")]
public class EndpointConfig : 
    IConfigureThisEndpoint, 
    AsA_Server
{
    public void Init()
    {
        Configure.Serialization.Json();
        Configure configure = Configure.With();
        configure.DefaultBuilder();
        configure.InMemorySagaPersister();
        configure.UseInMemoryTimeoutPersister();
        configure.InMemorySubscriptionStorage();
    }
}
