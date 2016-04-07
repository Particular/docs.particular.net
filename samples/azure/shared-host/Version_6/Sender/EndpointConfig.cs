using NServiceBus;
using NServiceBus.Logging;

public class EndpointConfig : IConfigureThisEndpoint
{
    public void Customize(BusConfiguration busConfiguration)
    {
        busConfiguration.UsePersistence<AzureStoragePersistence>();
        busConfiguration.UseTransport<AzureStorageQueueTransport>()
            .ConnectionString("UseDevelopmentStorage=true");

        LogManager.Use<DefaultFactory>().Level(LogLevel.Debug);

        busConfiguration.DisableNotUsedFeatures();
    }
}
