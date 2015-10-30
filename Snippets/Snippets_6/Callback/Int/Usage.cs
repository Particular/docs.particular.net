namespace Snippets6.Callback.Int
{
    using System;
    using NServiceBus;

    class Usage
    {
        async void Simple()
        {
            IBus bus = null;
            SendOptions sendOptions = new SendOptions();
            #region IntCallback

            Message message = new Message();
            int response = await bus.Request<int>(message, sendOptions);
            Console.WriteLine("Callback received with response:" + response);

            #endregion
        }

    }
}