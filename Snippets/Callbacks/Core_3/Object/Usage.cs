namespace Core3.Object
{
    using log4net;
    using NServiceBus;

    class Usage
    {

        Usage(IBus bus, ILog log)
        {
            #region ObjectCallback

            var message = new Message();
            bus.Send(message)
                .Register(ar =>
                {
                    var localResult = (CompletionResult)ar.AsyncState;
                    var response = (ResponseMessage)localResult.Messages[0];
                    log.Info($"Callback received with response:{response.Property}");
                }, null);

            #endregion
        }

    }
}