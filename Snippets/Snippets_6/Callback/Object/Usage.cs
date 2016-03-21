namespace Snippets6.Callback.Object
{
    using System;
    using NServiceBus;

    class Usage
    {
        async void Simple(EndpointConfiguration endpointConfiguration, IEndpointInstance endpoint, SendOptions sendOptions)
        {
            #region Callbacks-InstanceId

            endpointConfiguration.ScaleOut().InstanceDiscriminator("uniqueId");

            #endregion

            #region ObjectCallback

            Message message = new Message();
            ResponseMessage response = await endpoint.Request<ResponseMessage>(message, sendOptions);
            Console.WriteLine("Callback received with response:" + response.Property);

            #endregion
        }

    }
}