using NServiceBus;

#region nservicebus-host

public class EndpointConfig : IConfigureThisEndpoint, AsA_Server, IWantCustomInitialization
{
    public void Init()
    {
        Configure configure = Configure.With();

        configure.DefineEndpointName("Samples.NServiceBus.Host");
        configure.Log4Net();
        configure.DefaultBuilder();
        configure.Sagas();
        configure.MsmqTransport();
        configure.InMemorySagaPersister();
        configure.RunTimeoutManagerWithInMemoryPersistence();
        configure.InMemorySubscriptionStorage();
        configure.JsonSerializer();
    }
}

#endregion