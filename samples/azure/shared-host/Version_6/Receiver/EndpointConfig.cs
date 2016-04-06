using NServiceBus;
using NServiceBus.Logging;

public class EndpointConfig : IConfigureThisEndpoint
{
    public void Customize(EndpointConfiguration configuration)
    {
        configuration.UsePersistence<AzureStoragePersistence>();
        configuration.UseTransport<AzureStorageQueueTransport>()
            .ConnectionString("UseDevelopmentStorage=true");
        configuration.SendFailedMessagesTo("error");
        LogManager.Use<DefaultFactory>().Level(LogLevel.Debug);

        configuration.DisableNotUsedFeatures();
    }
}
