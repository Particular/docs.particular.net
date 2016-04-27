using NServiceBus;
using NServiceBus.Logging;

public class EndpointConfig : IConfigureThisEndpoint
{
    public void Customize(EndpointConfiguration configuration)
    {
        var persistence = configuration.UsePersistence<AzureStoragePersistence>();
        persistence.ConnectionString("UseDevelopmentStorage=true");
        var transport = configuration.UseTransport<AzureStorageQueueTransport>();
        transport.ConnectionString("UseDevelopmentStorage=true");
        transport.SerializeMessageWrapperWith<JsonSerializer>();
        configuration.SendFailedMessagesTo("error");
        LogManager.Use<DefaultFactory>().Level(LogLevel.Debug);

        configuration.DisableNotUsedFeatures();
    }
}
