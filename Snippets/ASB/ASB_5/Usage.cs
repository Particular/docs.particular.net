namespace ASB_5
{
    using NServiceBus;

    class Usage
    {
        Usage(Configure configure)
        {
            #region AzureServiceBusTransportWithAzure

            configure.UseTransport<AzureServiceBus>();

            #endregion
        }
        //TODO: fix when azure host is split
        /**
        #region AzureServiceBusTransportWithAzureHost

        public class EndpointConfig : IConfigureThisEndpoint, UsingTransport<AzureServiceBus>
        {
        }

        #endregion
    **/
    }
}