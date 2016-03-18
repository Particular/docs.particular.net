using NServiceBus;
[EndpointName("Samples.Logging.HostProfiles")]
public class EndpointConfig :
    IConfigureThisEndpoint,
    AsA_Server
{
    public void Customize(EndpointConfiguration endpointConfiguration)
    {
        endpointConfiguration.UseSerialization<JsonSerializer>();
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
    }
}