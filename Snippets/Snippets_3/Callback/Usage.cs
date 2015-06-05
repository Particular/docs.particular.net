namespace Snippets3.Callback
{
    using System;
    using NServiceBus;

    public class Usage
    {

        public Usage()
        {
            PlaceOrder placeOrder = new PlaceOrder();
            IBus bus = null;
            // ReSharper disable once NotAccessedVariable
            PlaceOrderResponse message; // get replied message

            #region CallbackToAccessMessageRegistration

            IAsyncResult sync = bus.Send(placeOrder)
                .Register(ar =>
                {
                    CompletionResult localResult = (CompletionResult) ar.AsyncState;
                    message = (PlaceOrderResponse) localResult.Messages[0];
                }, null);

            sync.AsyncWaitHandle.WaitOne();
            // return message;

            #endregion
            #region TriggerCallback

            bus.Return(Status.OK);

            #endregion

        }
    }
}