using NServiceBus;

class EnableTimeoutManagerCompatibility
{
    public EnableTimeoutManagerCompatibility(EndpointConfiguration endpointConfiguration)
    {
        #region 5to6-enable-timeout-manager-compatibility

        var transport = endpointConfiguration.UseTransport<SqlServerTransport>();
        var delayedDelivery = transport.NativeDelayedDelivery();
        delayedDelivery.EnableTimeoutManagerCompatibility();

        #endregion
    }
}