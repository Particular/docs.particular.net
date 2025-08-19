using System;
using NServiceBus;

class DelayedDelivery
{
    void Configure(EndpointConfiguration endpointConfiguration)
    {
        var transport = new SqlServerTransport("");

        #region DelayedDeliveryTableSuffix

        transport.DelayedDelivery.TableSuffix = "Delayed";

        #endregion

        #region DelayedDeliveryBatchSize

        transport.DelayedDelivery.BatchSize = 100;

        #endregion
    }
}