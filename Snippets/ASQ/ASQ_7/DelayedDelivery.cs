// ReSharper disable SuggestVarOrType_Elsewhere

using NServiceBus;

class DelayedDelivery
{
    public void DelayedDeliveryNameOverride(EndpointConfiguration endpointConfiguration)
    {
        #region delayed-delivery-override-name [7.4,)

        var transport = endpointConfiguration.UseTransport<AzureStorageQueueTransport>();
        var delayedDelivery = transport.DelayedDelivery();
        delayedDelivery.UseTableName("myendpoint");

        #endregion
    }
    public void DelayedDeliveryDisableTimeoutManager(EndpointConfiguration endpointConfiguration)
    {
        #region delayed-delivery-disable-timeoutmanager [7.4,)

        var transport = endpointConfiguration.UseTransport<AzureStorageQueueTransport>();
        var delayedDelivery = transport.DelayedDelivery();
        delayedDelivery.DisableTimeoutManager();

        #endregion
    }


}
