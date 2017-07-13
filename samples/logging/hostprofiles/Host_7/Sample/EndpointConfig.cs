using NServiceBus;
[EndpointName("Samples.Logging.HostProfiles")]
public class EndpointConfig :
    IConfigureThisEndpoint,
    AsA_Server
{
    public void Customize(EndpointConfiguration endpointConfiguration)
    {
        endpointConfiguration.UsePersistence<LearningPersistence>();
        endpointConfiguration.UseTransport<LearningTransport>();
    }
}