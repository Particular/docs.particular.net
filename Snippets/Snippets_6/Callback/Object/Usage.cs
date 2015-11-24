namespace Snippets6.Callback.Object
{
    using System;
    using NServiceBus;

    class Usage
    {
        async void Simple()
        {
            IBusContext busContext = null;
            SendOptions sendOptions = new SendOptions();
            #region ObjectCallback

            Message message = new Message();
            ResponseMessage response = await busContext.Request<ResponseMessage>(message, sendOptions);
            Console.WriteLine("Callback received with response:" + response.Property);

            #endregion
        }

    }
}