using NServiceBus;
using NServiceBus.Azure.Transports.WindowsAzureStorageQueues;
using NServiceBus.Logging;

public class EndpointConfig : IConfigureThisEndpoint
{
    public void Customize(EndpointConfiguration configuration)
    {
        configuration.UsePersistence<AzureStoragePersistence>()
            .ConnectionString("UseDevelopmentStorage=true");
        configuration.UseTransport<AzureStorageQueueTransport>()
            .ConnectionString("UseDevelopmentStorage=true")
            .SerializeMessageWrapperWith(definition => MessageWrapperSerializer.Json.Value);
        configuration.SendFailedMessagesTo("error");
        LogManager.Use<DefaultFactory>().Level(LogLevel.Debug);

        configuration.DisableNotUsedFeatures();
    }
}
