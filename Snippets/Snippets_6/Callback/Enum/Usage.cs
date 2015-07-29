namespace Snippets5.Callback.Enum
{
    using System;
    using NServiceBus;

    class Usage
    {
        async void Simple()
        {
            IBus bus = null;
            SendOptions sendOptions = new SendOptions();
            #region EnumCallback
            Message message = new Message();
            Status response = await bus.RequestWithTransientlyHandledResponse<Status>(message, sendOptions);
            Console.WriteLine("Callback received with response:" + response);
            #endregion
        }


    }
}