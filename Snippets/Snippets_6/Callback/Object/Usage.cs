namespace Snippets6.Callback.Object
{
    using System;
    using NServiceBus;

    class Usage
    {
        async void Simple()
        {
            IBusSession busSession = null;
            SendOptions sendOptions = new SendOptions();
            #region ObjectCallback

            Message message = new Message();
            ResponseMessage response = await busSession.Request<ResponseMessage>(message, sendOptions);
            Console.WriteLine("Callback received with response:" + response.Property);

            #endregion
        }

    }
}