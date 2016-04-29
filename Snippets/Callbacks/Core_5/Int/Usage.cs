namespace Core5.Int
{
    using NServiceBus;
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