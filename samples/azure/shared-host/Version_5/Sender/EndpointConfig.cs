using NServiceBus;
using NServiceBus.Logging;

public class EndpointConfig : IConfigureThisEndpoint
{
    public void Customize(BusConfiguration configuration)
    {
        configuration.UsePersistence<AzureStoragePersistence>();
        configuration.UseTransport<AzureStorageQueueTransport>()
            .ConnectionString("UseDevelopmentStorage=true");

        LogManager.Use<DefaultFactory>().Level(LogLevel.Debug);

        configuration.DisableNotUsedFeatures();
    }
}
