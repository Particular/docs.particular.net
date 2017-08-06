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

    public void Customize(EndpointConfiguration endpointConfiguration)
    {
        var persistence = endpointConfiguration.UsePersistence<AzureStoragePersistence>();
        persistence.ConnectionString("UseDevelopmentStorage=true");

        #region AzureMultiHost_MessageMapping

        var transport = endpointConfiguration.UseTransport<AzureStorageQueueTransport>();
        var routing = transport.Routing();
        routing.RouteToEndpoint(
            messageType: typeof(Ping),
            destination: "Receiver");

        #endregion

        transport.ConnectionString("UseDevelopmentStorage=true");
        transport.SerializeMessageWrapperWith<JsonSerializer>();
        transport.DelayedDelivery().DisableTimeoutManager();
        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.DisableNotUsedFeatures();
    }
}