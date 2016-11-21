using NServiceBus;
using NServiceBus.Logging;

public class EndpointConfig : IConfigureThisEndpoint
{
    public void Customize(EndpointConfiguration endpointConfiguration)
    {
        var persistence = endpointConfiguration.UsePersistence<AzureStoragePersistence>();
        persistence.ConnectionString("UseDevelopmentStorage=true");
        var transport = endpointConfiguration.UseTransport<AzureStorageQueueTransport>();
        transport.ConnectionString("UseDevelopmentStorage=true");
        transport.SerializeMessageWrapperWith<JsonSerializer>();
        endpointConfiguration.SendFailedMessagesTo("error");
        LogManager.Use<DefaultFactory>().Level(LogLevel.Debug);

        endpointConfiguration.DisableNotUsedFeatures();
    }
}