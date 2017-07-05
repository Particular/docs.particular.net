using System;
using NServiceBus;
using NServiceBus.Transport.SQLServer;

class DelayedDelivery
{
    void Configure(EndpointConfiguration endpointConfiguration)
    {
        #region EnableNativeDelayedDelivery

        var transport = endpointConfiguration.UseTransport<SqlServerTransport>();
        var delayeeDeliverySettings = transport.UseNativeDelayedDelivery();

        #endregion

        #region DelayedDeliveryTableSuffix

        delayeeDeliverySettings.TableSuffix("DelayedMessages");

        #endregion

        #region DelayedDeliveryProcessingInterval

        delayeeDeliverySettings.ProcessingInterval(TimeSpan.FromSeconds(15));

        #endregion

        #region DelayedDeliveryBatchSize

        delayeeDeliverySettings.BatchSize(50);

        #endregion

        #region DelayedDeliveryDisableTM

        delayeeDeliverySettings.DisableTimeoutManagerCompatibility();

        #endregion
    }
}
