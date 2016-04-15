namespace Snippets6.Callback.Int
{
    using NServiceBus;
    using NServiceBus.Logging;

    class Usage
    {
        async void Simple(IEndpointInstance endpoint, SendOptions sendOptions, ILog log)
        {

            #region IntCallback

            Message message = new Message();
            int response = await endpoint.Request<int>(message, sendOptions);
            log.Info("Callback received with response:" + response);

            #endregion
        }

    }
}