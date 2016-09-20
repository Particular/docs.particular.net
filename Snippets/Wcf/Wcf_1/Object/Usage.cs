namespace Wcf1.Object
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


        }
        #region WcfObjectCallback

        class MyService : WcfService<Message, ResponseMessage>
        {
        }

        #endregion

    }
}
