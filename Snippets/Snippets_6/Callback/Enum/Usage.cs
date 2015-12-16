namespace Snippets6.Callback.Enum
{
    using System;
    using NServiceBus;

    class Usage
    {
        async void Simple()
        {
            IBusSession busSession = null;
            SendOptions sendOptions = new SendOptions();
            #region EnumCallback
            Message message = new Message();
            Status response = await busSession.Request<Status>(message, sendOptions);
            Console.WriteLine("Callback received with response:" + response);
            #endregion
        }


    }
}