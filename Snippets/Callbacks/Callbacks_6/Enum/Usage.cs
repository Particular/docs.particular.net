namespace Callbacks.Enum
{
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.Logging;

    class Usage
    {
        async Task Simple(IMessageSession messageSession, ILog log)
        {
            #region EnumCallback

            var message = new Message();
            var response = await messageSession.Request<Status>(message);
            log.Info($"Callback received with response:{response}");

            #endregion
        }
    }
}