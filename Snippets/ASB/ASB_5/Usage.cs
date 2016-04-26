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
        #region AzureServiceBusTransportWithAzureHost

        public class EndpointConfig : IConfigureThisEndpoint,
            UsingTransport<AzureServiceBus>
        {
        }

        #endregion
    }
}