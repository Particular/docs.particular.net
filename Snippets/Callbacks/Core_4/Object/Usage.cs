namespace Core4.Object
{
    using NServiceBus;
    using NServiceBus.Logging;

    class Usage
    {

        Usage(IBus bus, ILog log)
        {
            #region ObjectCallback

            var message = new Message();
            bus.Send(message)
                .Register(
                    callback: asyncResult =>
                    {
                        var localResult = (CompletionResult) asyncResult.AsyncState;
                        var response = (ResponseMessage) localResult.Messages[0];
                        log.Info($"Callback received with response:{response.Property}");
                    },
                    state: null);

            #endregion
        }

    }
}