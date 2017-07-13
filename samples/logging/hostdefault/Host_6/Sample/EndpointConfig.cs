using NServiceBus;

[EndpointName("Samples.Logging.HostDefault")]
public class EndpointConfig :
    IConfigureThisEndpoint,
    AsA_Server
{
    public void Customize(BusConfiguration busConfiguration)
    {
        busConfiguration.EnableInstallers();
        busConfiguration.UsePersistence<InMemoryPersistence>();
    }
}
