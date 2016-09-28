namespace Core5.Enum
{
    using NServiceBus;
    using NServiceBus.Logging;

    class Usage
    {

        Usage(IBus bus, ILog log)
        {
            #region EnumCallback

            var message = new Message();
            bus.Send(message)
                .Register<Status>(
                    callback: response =>
                    {
                        log.Info($"Callback received with status:{response}");
                    });

            #endregion
        }

    }
}