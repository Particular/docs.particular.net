// namespace check disabled as it conflicts for the doco snippet Snippets4.Transports usage of AzureServiceBus type
// ReSharper disable once CheckNamespace
namespace Snippets4.Transports.Azure.AzureServiceBus
{
    using NServiceBus;

    class Usage
    {
        public Usage()
        {
            #region AzureServiceBusTransportWithAzure 5

            Configure.With()
                .UseTransport<AzureServiceBus>();

            #endregion
        }

        #region AzureServiceBusTransportWithAzureHost 5

        public class EndpointConfig : IConfigureThisEndpoint, UsingTransport<AzureServiceBus> { }

        #endregion
    }
}