namespace Snippets5.Callback.Object
{
    using System;
    using NServiceBus;

    class Usage
    {
        async void Simple()
        {
            IBus bus = null;
            SendOptions sendOptions = new SendOptions();
            #region ObjectCallback

            Message message = new Message();
            ResponseMessage response = await bus.RequestWithTransientlyHandledResponse<ResponseMessage>(message, sendOptions);
            Console.WriteLine("Callback received with response:" + response.Property);

            #endregion
        }

    }
}