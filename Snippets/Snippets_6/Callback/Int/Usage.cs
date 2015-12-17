namespace Snippets6.Callback.Int
{
    using System;
    using NServiceBus;

    class Usage
    {
        async void Simple()
        {
            IBusSession busSession = null;
            SendOptions sendOptions = new SendOptions();
            #region IntCallback

            Message message = new Message();
            int response = await busSession.Request<int>(message, sendOptions);
            Console.WriteLine("Callback received with response:" + response);

            #endregion
        }

    }
}