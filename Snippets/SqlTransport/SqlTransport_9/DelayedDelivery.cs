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

        #region DelayedDeliveryBatchSizeCustom

        transport.DelayedDelivery.BatchSize = 50;

        #endregion
    }

    void UseIndependentMovers()
    {
        #region UseIndependentMoversSwitch

        AppContext.SetSwitch("NServiceBus.Transport.SqlServer.DelayedDelivery.UseIndependentMovers", true);

        #endregion
    }
}