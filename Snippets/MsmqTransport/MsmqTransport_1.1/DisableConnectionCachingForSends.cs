using NServiceBus;

class DisableConnectionCachingForSends
{
    DisableConnectionCachingForSends(EndpointConfiguration endpointConfiguration)
    {
        #region disable-connection-caching-for-sends

        var transport = endpointConfiguration.UseTransport<MsmqTransport>();
        transport.DisableConnectionCachingForSends();

        #endregion
    }
}
