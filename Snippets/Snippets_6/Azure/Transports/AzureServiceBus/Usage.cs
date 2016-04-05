namespace Snippets6.Azure.Transports.AzureServiceBus
{
    using NServiceBus;

    class Usage
    {
        Usage(EndpointConfiguration endpointConfiguration)
        {
            #region AzureServiceBusTransportWithAzure 7

            endpointConfiguration.UseTransport<AzureServiceBusTransport>();

            #endregion
        }

        #region AzureServiceBusTransportWithAzureHost 7

        public class EndpointConfig : IConfigureThisEndpoint
        {
            public void Customize(EndpointConfiguration endpointConfiguration)
            {
                endpointConfiguration.UseTransport<AzureServiceBusTransport>();
            }
        }

        #endregion
    }
}