using System;
using NServiceBus;

class DelayConfig
{
    void ConfigurePeekDelay(EndpointConfiguration endpointConfiguration)
    {
        #region sqlserver-queue-peeker-config-delay

        var transport = new SqlServerTransport("connectionString")
        {
            QueuePeeker =
            {
                Delay = TimeSpan.FromSeconds(5)
            }
        };

        #endregion
    }

    void ConfigurePeekBatchSize(EndpointConfiguration endpointConfiguration)
    {
        #region sqlserver-queue-peeker-config-batch-size

        var transport = new SqlServerTransport("connectionString")
        {
            QueuePeeker =
            {
                MaxRecordsToPeek = 50
            }
        };

        #endregion
    }
}