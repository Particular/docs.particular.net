using NServiceBus;

class DiscardMessages
{
    void Configure(EndpointConfiguration endpointConfiguration)
    {
        #region purge-expired-on-startup

        var transport = new SqlServerTransport("connectionString");
        transport.ExpiredMessagesPurger.PurgeOnStartup = true;
        transport.ExpiredMessagesPurger.PurgeBatchSize = 5000;

        #endregion
    }
}