namespace Core4.Int
{
    using NServiceBus;
    using NServiceBus.Logging;

    class Usage
    {

        Usage(IBus bus, ILog log)
        {
            #region IntCallback

            var message = new Message();
            bus.Send(message)
                .Register<int>(
                    callback: response =>
                    {
                        log.Info($"Callback received with response:{response}");
                    });

            #endregion
        }

    }
}