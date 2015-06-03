using NServiceBus;

class ConfigureAzureServiceBusTransport
{
    public void Demo()
    {
        #region AzureServiceBusTransportWithAzure 5

        Configure.With()
                 .UseTransport<AzureServiceBus>();

        #endregion
    }
}