using NServiceBus;

public class EndpointConfig : 
    IConfigureThisEndpoint, 
    AsA_Server
{
    public void Init()
    {
        Configure.Serialization.Json();
        Configure configure = Configure.With();
        configure.DefineEndpointName("Samples.Logging.HostProfiles");
        configure.DefaultBuilder();
        configure.InMemorySagaPersister();
        configure.UseInMemoryTimeoutPersister();
        configure.InMemorySubscriptionStorage();
    }
}
