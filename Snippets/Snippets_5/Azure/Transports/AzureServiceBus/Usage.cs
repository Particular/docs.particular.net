namespace Snippets5.Azure.Transports.AzureServiceBus
{
    using NServiceBus;

    class Usage
    {
        public Usage()
        {
            #region AzureServiceBusTransportWithAzure 6

            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.UseTransport<AzureServiceBusTransport>();

            #endregion
        }

        #region AzureServiceBusTransportWithAzureHost 6

        public class EndpointConfig : IConfigureThisEndpoint
        {
            public void Customize(BusConfiguration busConfiguration)
            {
                busConfiguration.UseTransport<AzureServiceBusTransport>();
            }
        }

        #endregion
    }
}