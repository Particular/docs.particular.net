namespace Core4.Enum
{
    using NServiceBus;
    using NServiceBus.Logging;

    class Usage
    {

        Usage(IBus bus, ILog log)
        {
            #region EnumCallback

            Message message = new Message();
            bus.Send(message)
                .Register<Status>(response =>
                {
                    log.Info("Callback received with response:" + response);
                });

            #endregion
        }
    }
}