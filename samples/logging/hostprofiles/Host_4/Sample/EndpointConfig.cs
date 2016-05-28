using NServiceBus;

[EndpointName("Samples.Logging.HostProfiles")]
public class EndpointConfig :
    IConfigureThisEndpoint,
    AsA_Server
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
