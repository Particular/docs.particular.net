using NServiceBus;

[EndpointName("Samples.Logging.HostProfiles")]
public class EndpointConfig : 
    IConfigureThisEndpoint, 
    AsA_Server
{
    public void Customize(BusConfiguration busConfiguration)
    {
        busConfiguration.UseSerialization<JsonSerializer>();
        busConfiguration.EnableInstallers();
        busConfiguration.UsePersistence<InMemoryPersistence>();
    }
}