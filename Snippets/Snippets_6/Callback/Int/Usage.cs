namespace Snippets6.Callback.Int
{
    using System;
    using NServiceBus;

    class Usage
    {
        async void Simple()
        {
            IEndpointInstance endpoint = null;
            SendOptions sendOptions = new SendOptions();
            #region IntCallback

            Message message = new Message();
            int response = await endpoint.Request<int>(message, sendOptions);
            Console.WriteLine("Callback received with response:" + response);

            #endregion
        }

    }
}