using System;
using NServiceBus;

class DelayConfig
{
    void ConfigurePeekDelay(EndpointConfiguration endpointConfiguration)
    {
        // ReSharper disable UseObjectOrCollectionInitializer
        #region sqlserver-queue-peeker-config-delay

        var transport = new SqlServerTransport("connectionString")
        {
            QueuePeeker = 
            {
                Delay = TimeSpan.FromSeconds(5)
            }
        };

        #endregion
        // ReSharper restore UseObjectOrCollectionInitializer
    }
    
    void ConfigurePeekBatchSize(EndpointConfiguration endpointConfiguration)
    {
        // ReSharper disable UseObjectOrCollectionInitializer
        #region sqlserver-queue-peeker-config-batch-size

        var transport = new SqlServerTransport("connectionString")
        {
            QueuePeeker = 
            {
                MaxRecordsToPeek = 50
            }
        };

        #endregion
        // ReSharper restore UseObjectOrCollectionInitializer
    }
}
