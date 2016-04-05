using NServiceBus;
using NServiceBus.Logging;

public class EndpointConfig : IConfigureThisEndpoint
{

    public void Customize(EndpointConfiguration endpointConfiguration)
    {
        endpointConfiguration.UsePersistence<AzureStoragePersistence>();
        endpointConfiguration.UseTransport<AzureStorageQueueTransport>()
            .ConnectionString("UseDevelopmentStorage=true");

        LogManager.Use<DefaultFactory>().Level(LogLevel.Debug);

        endpointConfiguration.DisableNotUsedFeatures();
    }
}
