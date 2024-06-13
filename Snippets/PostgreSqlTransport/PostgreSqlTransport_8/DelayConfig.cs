using System;
using NServiceBus;

class DelayConfig
{
    void ConfigurePeekDelay(EndpointConfiguration endpointConfiguration)
    {
        #region postgresql-queue-peeker-config-delay

        var transport = new PostgreSqlTransport("connectionString")
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
        #region postgresql-queue-peeker-config-batch-size

        var transport = new PostgreSqlTransport("connectionString")
        {
            QueuePeeker =
            {
                MaxRecordsToPeek = 50
            }
        };

        #endregion
    }
}