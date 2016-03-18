namespace Snippets6.Callback.Object
{
    using System;
    using NServiceBus;

    class Usage
    {
        async void Simple(EndpointConfiguration endpointConfiguration)
        {
            #region Callbacks-InstanceId
            endpointConfiguration.ScaleOut().InstanceDiscriminator("uniqueId");
            #endregion

            IEndpointInstance endpoint = null;
            SendOptions sendOptions = new SendOptions();
            #region ObjectCallback

            Message message = new Message();
            ResponseMessage response = await endpoint.Request<ResponseMessage>(message, sendOptions);
            Console.WriteLine("Callback received with response:" + response.Property);

            #endregion
        }

    }
}