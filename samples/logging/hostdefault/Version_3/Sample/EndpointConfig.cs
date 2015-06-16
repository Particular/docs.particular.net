using NServiceBus;

public class EndpointConfig :
    IConfigureThisEndpoint, 
    AsA_Server, 
    IWantCustomInitialization
{
    public void Init()
    {
        Configure configure = Configure.With();
        configure.DefineEndpointName("Samples.Logging.HostDefault");
        configure.DefaultBuilder();
        configure.InMemorySagaPersister();
        configure.RunTimeoutManagerWithInMemoryPersistence();
        configure.InMemorySubscriptionStorage();
        configure.JsonSerializer();
    }
}