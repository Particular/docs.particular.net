namespace Callbacks.UpgradeGuides._1to2
{
    using System.Configuration;
    using NServiceBus;

    class Callbacks
    {
        void Simple(EndpointConfiguration endpointConfiguration, IEndpointInstance endpoint)
        {
            #region 1to2-Callbacks-InstanceId-Default

            var instanceDiscriminator = ConfigurationManager.AppSettings["InstanceId"];
            endpointConfiguration.MakeInstanceUniquelyAddressable(instanceDiscriminator);
            endpointConfiguration.EnableCallbacks();
            #endregion

            #region 1to2-Callbacks-InstanceId-NoRequests

            endpointConfiguration.EnableCallbacks(makesRequests: false);
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