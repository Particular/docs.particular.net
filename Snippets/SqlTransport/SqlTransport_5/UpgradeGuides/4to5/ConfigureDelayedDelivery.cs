using System;
using NServiceBus;
using NServiceBus.Transport.SQLServer;

class ConfigureDelayedDelivery
{
    public ConfigureDelayedDelivery(EndpointConfiguration endpointConfiguration)
    {
        #region 4to5-configure-native-delayed-delivery

        var transport = endpointConfiguration.UseTransport<SqlServerTransport>();
        var delayedDelivery = transport.NativeDelayedDelivery();

        delayedDelivery.BatchSize(100);
        delayedDelivery.DisableTimeoutManagerCompatibility();
        delayedDelivery.ProcessingInterval(TimeSpan.FromSeconds(5));
        delayedDelivery.TableSuffix("Delayed");

        #endregion
    }
}