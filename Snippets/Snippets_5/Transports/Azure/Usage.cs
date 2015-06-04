namespace Snippets5.Transports.Azure
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
    }
}