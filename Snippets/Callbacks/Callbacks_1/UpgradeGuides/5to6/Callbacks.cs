namespace Core6.UpgradeGuides._5to6
{
    using System.Threading.Tasks;
    using NServiceBus;

    class Callbacks
    {
        async Task Simple(EndpointConfiguration endpointConfiguration, IEndpointInstance endpoint, SendOptions sendOptions)
        {
            #region 5to6-Callbacks-InstanceId
            endpointConfiguration.MakeInstanceUniquelyAddressable("uniqueId");
            #endregion

            #region 5to6-Callbacks

            var message = new RequestMessage();
            var response = await endpoint.Request<ResponseMessage>(message, sendOptions)
                .ConfigureAwait(false);

            #endregion
        }

        class RequestMessage :
            IMessage
        {

        }

        class ResponseMessage :
            IMessage
        {

        }
    }
}