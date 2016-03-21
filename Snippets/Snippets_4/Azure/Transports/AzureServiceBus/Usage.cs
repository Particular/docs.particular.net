namespace Snippets4.Azure.Transports.AzureServiceBus
{
    using NServiceBus;

    class Usage
    {
        Usage(Configure configure)
        {
            #region AzureServiceBusTransportWithAzure 5

            configure.UseTransport<AzureServiceBus>();

            #endregion
        }

        #region AzureServiceBusTransportWithAzureHost 5

        public class EndpointConfig : IConfigureThisEndpoint, UsingTransport<AzureServiceBus>
        {
        }

        #endregion
    }
}