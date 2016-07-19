using NServiceBus;
using NServiceBus.Features;

#region nservicebus-host
[EndpointName("Samples.NServiceBus.Host")]
public class EndpointConfig :
    IConfigureThisEndpoint,
    AsA_Server,
    IWantCustomInitialization
{
    public void Init()
    {
        Configure.Serialization.Json();
        Configure.Features.Enable<Sagas>();

        var configure = Configure.With();

        configure.DefineEndpointName("Samples.NServiceBus.Host");
        configure.DefaultBuilder();
        configure.InMemorySagaPersister();
        configure.UseInMemoryTimeoutPersister();
        configure.InMemorySubscriptionStorage();
        configure.UseTransport<Msmq>();
    }
}

#endregion