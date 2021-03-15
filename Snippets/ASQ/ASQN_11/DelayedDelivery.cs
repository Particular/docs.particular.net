// ReSharper disable SuggestVarOrType_Elsewhere

using NServiceBus;

class DelayedDelivery
{
    public void DelayedDeliveryNameOverride(EndpointConfiguration endpointConfiguration)
    {
        #region delayed-delivery-override-name

        var transport = new AzureStorageQueueTransport("connection string");
        transport.DelayedDelivery.DelayedDeliveryTableName = "myendpoint";

        endpointConfiguration.UseTransport(transport);

        #endregion
    }
    public void DelayedDeliveryDisableTimeoutManager(EndpointConfiguration endpointConfiguration)
    {
        #region delayed-delivery-disable-timeoutmanager

        #endregion
    }

    public void DisableDelayedDelivery(EndpointConfiguration endpointConfiguration)
    {
        #region delayed-delivery-disabled

        var transport = new AzureStorageQueueTransport("connection string", useNativeDelayedDeliveries: false);
        endpointConfiguration.UseTransport(transport);

        #endregion
    }
}
