namespace Core4.Object
{
    using NServiceBus;
    using NServiceBus.Logging;

    class Usage
    {

        Usage(IBus bus, ILog log)
        {
            #region ObjectCallback

            Message message = new Message();
            bus.Send(message)
                .Register(ar =>
                {
                    CompletionResult localResult = (CompletionResult)ar.AsyncState;
                    ResponseMessage response = (ResponseMessage)localResult.Messages[0];
                    log.Info("Callback received with response:" + response.Property);
                }, null);

            #endregion
        }

    }
}