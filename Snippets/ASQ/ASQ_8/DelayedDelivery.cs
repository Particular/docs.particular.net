// ReSharper disable SuggestVarOrType_Elsewhere

using NServiceBus;

class DelayedDelivery
{
    public void DelayedDeliveryNameOverride(EndpointConfiguration endpointConfiguration)
    {
        #region delayed-delivery-override-name

        var transport = endpointConfiguration.UseTransport<AzureStorageQueueTransport>();
        var delayedDelivery = transport.DelayedDelivery();
        delayedDelivery.UseTableName("myendpoint");

        #endregion
    }
    public void DelayedDeliveryDisableTimeoutManager(EndpointConfiguration endpointConfiguration)
    {
        #region delayed-delivery-disable-timeoutmanager

        var transport = endpointConfiguration.UseTransport<AzureStorageQueueTransport>();
        var delayedDelivery = transport.DelayedDelivery();
        delayedDelivery.DisableTimeoutManager();

        #endregion
    }

    public void DisableDelayedDelivery(EndpointConfiguration endpointConfiguration)
    {
        #region delayed-delivery-disabled

        var transport = endpointConfiguration.UseTransport<AzureStorageQueueTransport>();
        var delayedDelivery = transport.DelayedDelivery();
        delayedDelivery.DisableDelayedDelivery();

        #endregion
    }
}
