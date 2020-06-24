using NServiceBus;

class DiscardMessages
{
    void Configure(EndpointConfiguration endpointConfiguration)
    {
        #region purge-expired-on-startup

        var transport = endpointConfiguration.UseTransport<SqlServerTransport>();
        transport.PurgeExpiredMessagesOnStartup(purgeBatchSize: 5000);

        #endregion
    }
}