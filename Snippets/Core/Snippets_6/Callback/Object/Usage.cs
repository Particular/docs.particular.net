namespace Snippets6.Callback.Object
{
    using NServiceBus;
    using NServiceBus.Logging;

    class Usage
    {
        async void Simple(EndpointConfiguration endpointConfiguration, IEndpointInstance endpoint, SendOptions sendOptions, ILog log)
        {
            #region Callbacks-InstanceId

            endpointConfiguration.ScaleOut().InstanceDiscriminator("uniqueId");

            #endregion

            #region ObjectCallback

            Message message = new Message();
            ResponseMessage response = await endpoint.Request<ResponseMessage>(message, sendOptions);
            log.Info("Callback received with response:" + response.Property);

            #endregion
        }

    }
}