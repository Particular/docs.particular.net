namespace Callbacks.Int
{
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.Logging;

    class Usage
    {
        async Task Simple(IEndpointInstance endpoint, ILog log)
        {

            #region IntCallback

            var message = new Message();
            var response = await endpoint.Request<int>(message)
                .ConfigureAwait(false);
            log.Info($"Callback received with response:{response}");

            #endregion
        }
    }
}