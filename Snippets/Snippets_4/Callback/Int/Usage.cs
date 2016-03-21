using NServiceBus;

namespace Snippets4.Callback.Int
{
    using NServiceBus.Logging;

    class Usage
    {

        Usage(IBus bus, ILog log)
        {
            #region IntCallback

            Message message = new Message();
            bus.Send(message)
                .Register<int>(response =>
                {
                    log.Info("Callback received with response:" + response);
                });

            #endregion
        }

    }
}