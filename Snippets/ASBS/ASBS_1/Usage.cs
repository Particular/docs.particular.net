using NServiceBus;

class Usage
{
    Usage(EndpointConfiguration endpointConfiguration)
    {
        #region azure-service-bus-for-dotnet-standard

        var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
        transport.ConnectionString("Endpoint=sb://[NAMESPACE].servicebus.windows.net/;SharedAccessKeyName=[KEYNAME];SharedAccessKey=[KEY]");

        #endregion

        #region custom-prefetch-multiplier

        transport.PrefetchMultiplier(3);

        #endregion

        #region custom-prefetch-count

        transport.PrefetchCount(100);

        #endregion
    }
}