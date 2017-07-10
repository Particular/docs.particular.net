using System;
using NServiceBus;
using NServiceBus.Transport.SQLServer;

class DelayedDelivery
{
    void Configure(EndpointConfiguration endpointConfiguration)
    {
        #region EnableNativeDelayedDelivery

        var transport = endpointConfiguration.UseTransport<SqlServerTransport>();
        var delayedDeliverySettings = transport.UseNativeDelayedDelivery();

        #endregion

        #region DelayedDeliveryTableSuffix

        delayedDeliverySettings.TableSuffix("DelayedMessages");

        #endregion

        #region DelayedDeliveryProcessingInterval

        delayedDeliverySettings.ProcessingInterval(TimeSpan.FromSeconds(15));

        #endregion

        #region DelayedDeliveryBatchSize

        delayedDeliverySettings.BatchSize(50);

        #endregion

        #region DelayedDeliveryDisableTM

        delayedDeliverySettings.DisableTimeoutManagerCompatibility();

        #endregion
    }
}
