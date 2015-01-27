using NServiceBus;

public class EndpointConfig : IConfigureThisEndpoint, AsA_Server
{
    public void Init()
    {
        Configure.Serialization.Json();
        var configure = Configure.With();
        configure.DefineEndpointName("HostProfilesLogging");
        configure.DefaultBuilder();
        configure.InMemorySagaPersister();
        configure.UseInMemoryTimeoutPersister();
        configure.InMemorySubscriptionStorage();
    }
}
