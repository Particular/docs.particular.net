using NServiceBus;
[EndpointName("Samples.Logging.HostDefault")]
public class EndpointConfig :
    IConfigureThisEndpoint,
    AsA_Server
{

    public void Customize(EndpointConfiguration endpointConfiguration)
    {
        endpointConfiguration.UseSerialization<JsonSerializer>();
        endpointConfiguration.UsePersistence<LearningPersistence>();
        endpointConfiguration.UseTransport<LearningTransport>();
    }
}

