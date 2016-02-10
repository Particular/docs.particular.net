namespace Snippets6.Callback.Enum
{
    using System;
    using NServiceBus;

    class Usage
    {
        async void Simple()
        {
            IEndpointInstance endpoint = null;
            SendOptions sendOptions = new SendOptions();
            #region EnumCallback
            Message message = new Message();
            Status response = await endpoint.Request<Status>(message, sendOptions);
            Console.WriteLine("Callback received with response:" + response);
            #endregion
        }


    }
}