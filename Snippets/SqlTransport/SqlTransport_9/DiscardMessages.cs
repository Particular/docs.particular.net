using NServiceBus;

class DiscardMessages
{
    void Configure(EndpointConfiguration endpointConfiguration)
    {
        #region purge-expired-on-startup

        var transport = new SqlServerTransport("connectionString")
        {
            ExpiredMessagesPurger = 
            {
                PurgeOnStartup = true,
                PurgeBatchSize = 5000
            }
        };

        #endregion
    }
}