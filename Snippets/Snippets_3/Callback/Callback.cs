using System;
using NServiceBus;

public class Callback
{
    class PlaceOrder : ICommand
    {
    }

    class PlaceOrderResponse : IMessage
    {
        public object Response { get; set; }
    }

    public void CallbackSnippet()
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
    }

    enum Status
    {
        OK,
        Error
    }

    public void TriggerCallback()
    {
        IBus bus = null;

        #region TriggerCallback

        bus.Return(Status.OK);

        #endregion

    }
}