using NServiceBus;

#region nservicebus-host

[EndpointName("Samples.NServiceBus.Host")]
public class EndpointConfig :
    IConfigureThisEndpoint
{
    public void Customize(BusConfiguration busConfiguration)
    {
        busConfiguration.EnableInstallers();
        busConfiguration.UsePersistence<InMemoryPersistence>();
    }
}

#endregion