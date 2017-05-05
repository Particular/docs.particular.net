namespace Callbacks.Object
{
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.Logging;

    class Usage
    {
        async Task Simple(EndpointConfiguration endpointConfiguration, IEndpointInstance endpoint, ILog log)
        {
            #region Callbacks-InstanceId

            endpointConfiguration.MakeInstanceUniquelyAddressable("uniqueId");

            #endregion

            #region ObjectCallback

            var message = new Message();
            var response = await endpoint.Request<ResponseMessage>(message)
                .ConfigureAwait(false);
            log.Info($"Callback received with response:{response.Property}");

            #endregion
        }
    }

}