namespace Snippets6.Callback.Enum
{
    using NServiceBus;
    using NServiceBus.Logging;

    class Usage
    {
        async void Simple(IEndpointInstance endpoint, SendOptions sendOptions, ILog log)
        {
            #region EnumCallback
            Message message = new Message();
            Status response = await endpoint.Request<Status>(message, sendOptions);
            log.Info("Callback received with response:" + response);
            #endregion
        }


    }
}