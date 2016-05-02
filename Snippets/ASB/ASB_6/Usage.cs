using NServiceBus;

class Usage
{
    Usage(BusConfiguration busConfiguration)
    {
        #region AzureServiceBusTransportWithAzure

        busConfiguration.UseTransport<AzureServiceBusTransport>();

        #endregion
    }

    #region AzureServiceBusTransportWithAzureHost

    public class EndpointConfig : IConfigureThisEndpoint
    {
        public void Customize(BusConfiguration busConfiguration)
        {
            busConfiguration.UseTransport<AzureServiceBusTransport>();
        }
    }

    #endregion
}