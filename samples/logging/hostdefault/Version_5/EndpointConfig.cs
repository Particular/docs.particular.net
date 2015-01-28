using NServiceBus;

public class EndpointConfig :
    IConfigureThisEndpoint,
    AsA_Server
{
    public void Customize(BusConfiguration configuration)
    {
        configuration.EndpointName("HostDefaultLoggingSample");
        configuration.UseSerialization<JsonSerializer>();
        configuration.EnableInstallers();
        configuration.UsePersistence<InMemoryPersistence>();
    }
}
