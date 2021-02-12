using NServiceBus;

class DisableConnectionCachingForSends
{
    DisableConnectionCachingForSends(EndpointConfiguration endpointConfiguration)
    {
        #region disable-connection-caching-for-sends

        var transport = new MsmqTransport
        {
            UseConnectionCache = false
        };
        endpointConfiguration.UseTransport(transport);

        #endregion
    }
}
