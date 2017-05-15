using NServiceBus;

#region nservicebus-host
[EndpointName("Samples.NServiceBus.Host")]
public class EndpointConfig :
    IConfigureThisEndpoint
{
    public void Customize(EndpointConfiguration endpointConfiguration)
    {
        endpointConfiguration.UseSerialization<JsonSerializer>();
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.UsePersistence<LearningPersistence>();
        endpointConfiguration.UseTransport<LearningTransport>();
    }
}

#endregion