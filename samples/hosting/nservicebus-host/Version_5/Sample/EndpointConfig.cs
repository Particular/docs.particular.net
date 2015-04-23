using NServiceBus;

#region nservicebus-host

public class EndpointConfig : IConfigureThisEndpoint
{
    public void Customize(BusConfiguration busConfiguration)
    {
        busConfiguration.EndpointName("Samples.NServiceBus.Host");
        busConfiguration.UseSerialization<JsonSerializer>();
        busConfiguration.EnableInstallers();
        busConfiguration.UsePersistence<InMemoryPersistence>();
    }
}

#endregion