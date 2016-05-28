namespace Core6.Int
{
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.Logging;

    class Usage
    {
        async Task Simple(IEndpointInstance endpoint, SendOptions sendOptions, ILog log)
        {

            #region IntCallback

            var message = new Message();
            var response = await endpoint.Request<int>(message, sendOptions)
                .ConfigureAwait(false);
            log.Info("Callback received with response:" + response);

            #endregion
        }

    }
}