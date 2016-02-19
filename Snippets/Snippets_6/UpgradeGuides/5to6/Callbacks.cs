namespace Snippets5.UpgradeGuides._5to6
{
    using System.Threading.Tasks;
    using NServiceBus;

    class Callbacks
    {
        async Task Simple()
        {
            #region 5to6-Callbacks-InstanceId
            EndpointConfiguration endpointConfiguration = new EndpointConfiguration();
            endpointConfiguration.ScaleOut().InstanceDiscriminator("uniqueId");
            #endregion

            IEndpointInstance endpoint = null;
            SendOptions sendOptions = new SendOptions();
            #region 5to6-Callbacks

            RequestMessage message = new RequestMessage();
            ResponseMessage response = await endpoint.Request<ResponseMessage>(message, sendOptions);

            #endregion
        }

        class RequestMessage : IMessage
        {
            
        }

        class ResponseMessage : IMessage
        {
             
        }
    }
}