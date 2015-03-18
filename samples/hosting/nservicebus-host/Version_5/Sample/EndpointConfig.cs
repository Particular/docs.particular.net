using NServiceBus;

#region nservicebus-host

public class EndpointConfig : IConfigureThisEndpoint
{
    public void Customize(BusConfiguration configuration)
    {
        configuration.EndpointName("Samples.NServiceBus.Host");
        configuration.UseSerialization<JsonSerializer>();
        configuration.EnableInstallers();
        configuration.UsePersistence<InMemoryPersistence>();
    }
}

#endregion