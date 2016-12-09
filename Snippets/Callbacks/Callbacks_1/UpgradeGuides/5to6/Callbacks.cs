namespace Core6.UpgradeGuides._5to6
{
    using System.Configuration;
    using System.Threading.Tasks;
    using NServiceBus;

    class Callbacks
    {
        async Task Simple(EndpointConfiguration endpointConfiguration, IEndpointInstance endpoint)
        {
            #region 5to6-Callbacks-InstanceId

            var instanceDiscriminator = ConfigurationManager.AppSettings["InstanceId"];
            endpointConfiguration.MakeInstanceUniquelyAddressable(instanceDiscriminator);
            #endregion

            #region 5to6-Callbacks

            var message = new RequestMessage();
            var response = await endpoint.Request<ResponseMessage>(message)
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