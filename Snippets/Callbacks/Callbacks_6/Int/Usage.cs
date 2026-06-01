namespace Callbacks.Int
{
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.Logging;

    class Usage
    {
        async Task Simple(IMessageSession messageSession, ILog log)
        {

            #region IntCallback

            var message = new Message();
            var response = await messageSession.Request<int>(message);
            log.Info($"Callback received with response:{response}");

            #endregion
        }
    }
}