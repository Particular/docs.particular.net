using NServiceBus;
using NServiceBus.Logging;

public class EndpointConfig :
    IConfigureThisEndpoint
{
    static EndpointConfig()
    {
        var defaultFactory = LogManager.Use<DefaultFactory>();
        defaultFactory.Level(LogLevel.Debug);
    }

    public void Customize(BusConfiguration busConfiguration)
    {
        busConfiguration.UsePersistence<AzureStoragePersistence>();
        var transport = busConfiguration.UseTransport<AzureStorageQueueTransport>();
        transport.ConnectionString("UseDevelopmentStorage=true");
        busConfiguration.DisableNotUsedFeatures();
    }
}
