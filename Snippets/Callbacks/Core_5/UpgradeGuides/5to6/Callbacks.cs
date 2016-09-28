namespace Core5.UpgradeGuides._5to6
{
    using NServiceBus;

    class Callbacks
    {

        Callbacks(IBus bus)
        {
            #region 5to6-Callbacks

            var requestMessage = new RequestMessage();
            bus.Send(requestMessage)
                .Register(
                    callback: asyncResult =>
                    {
                        var localResult = (CompletionResult) asyncResult.AsyncState;
                        var response = (ResponseMessage) localResult.Messages[0];
                    },
                    state: null);

            #endregion
        }

        class RequestMessage :
            IMessage
        {
        }

        class ResponseMessage :
            IMessage
        {
        }
    }
}