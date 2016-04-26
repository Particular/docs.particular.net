using NServiceBus;
using NServiceBus.Logging;

public class EndpointConfig : IConfigureThisEndpoint
{
    public void Customize(EndpointConfiguration configuration)
    {
        configuration.UsePersistence<AzureStoragePersistence>()
            .ConnectionString("UseDevelopmentStorage=true");
        configuration.UseTransport<AzureStorageQueueTransport>()
            .ConnectionString("UseDevelopmentStorage=true")
            .SerializeMessageWrapperWith<JsonSerializer>();
        configuration.SendFailedMessagesTo("error");
        LogManager.Use<DefaultFactory>()
            .Level(LogLevel.Debug);

        configuration.DisableNotUsedFeatures();
    }
}
