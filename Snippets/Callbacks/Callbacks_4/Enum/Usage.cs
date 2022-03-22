namespace Callbacks.Enum
{
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.Logging;

    class Usage
    {
        async Task Simple(IEndpointInstance endpoint, ILog log)
        {
            #region EnumCallback

            var message = new Message();
            var response = await endpoint.Request<Status>(message)
                .ConfigureAwait(false);
            log.Info($"Callback received with response:{response}");

            #endregion
        }
    }
}