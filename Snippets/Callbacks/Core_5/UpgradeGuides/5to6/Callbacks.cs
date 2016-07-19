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
                .Register(ar =>
                {
                    var localResult = (CompletionResult) ar.AsyncState;
                    var response = (ResponseMessage) localResult.Messages[0];
                }, null);

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