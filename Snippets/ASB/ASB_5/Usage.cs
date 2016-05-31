using NServiceBus;

class Usage
{
    Usage(Configure configure)
    {
        #region AzureServiceBusTransportWithAzure

        configure.UseTransport<AzureServiceBus>();

        #endregion
    }

}