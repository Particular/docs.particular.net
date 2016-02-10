using NServiceBus;

public class EndpointConfig : IConfigureThisEndpoint,
    AsA_Server
{

    public void Customize(EndpointConfiguration endpointConfiguration)
    {
        endpointConfiguration.EndpointName("Samples.Logging.HostDefault");
        endpointConfiguration.UseSerialization<JsonSerializer>();
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
    }
}

