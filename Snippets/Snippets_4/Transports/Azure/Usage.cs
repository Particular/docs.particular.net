namespace Snippets4.Transports.Azure
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
    }
}