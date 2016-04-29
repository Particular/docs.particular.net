namespace Core3.Enum
{
    using log4net;
    using NServiceBus;

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