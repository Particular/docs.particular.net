namespace Snippets6.Callback.Object
{
    using System;
    using NServiceBus;

    class Usage
    {
        async void Simple()
        {
            IEndpointInstance endpointInstance = null;
            SendOptions sendOptions = new SendOptions();
            #region ObjectCallback

            Message message = new Message();
            ResponseMessage response = await endpointInstance.Request<ResponseMessage>(message, sendOptions);
            Console.WriteLine("Callback received with response:" + response.Property);

            #endregion
        }

    }
}