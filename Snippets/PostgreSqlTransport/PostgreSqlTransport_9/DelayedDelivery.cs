using NServiceBus;

class DelayedDelivery
{
    void Configure(EndpointConfiguration endpointConfiguration)
    {
        var transport = new PostgreSqlTransport("");

        #region DelayedDeliveryTableSuffix

        transport.DelayedDelivery.TableSuffix = "Delayed";

        #endregion

        #region DelayedDeliveryBatchSize

        transport.DelayedDelivery.BatchSize = 100;

        #endregion
    }
}