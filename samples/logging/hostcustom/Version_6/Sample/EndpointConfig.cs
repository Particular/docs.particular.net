using NServiceBus;
using NServiceBus.Logging;

#region Config
public class EndpointConfig : 
    IConfigureThisEndpoint, 
    AsA_Server
{
    public EndpointConfig()
    {
        DefaultFactory defaultFactory = LogManager.Use<DefaultFactory>();
        defaultFactory.Level(LogLevel.Info);
    }
#endregion

    public void Customize(BusConfiguration configuration)
    {
        configuration.EndpointName("Samples.Logging.HostCustom");
        configuration.UseSerialization<JsonSerializer>();
        configuration.EnableInstallers();
        configuration.UsePersistence<InMemoryPersistence>();
    }
}
