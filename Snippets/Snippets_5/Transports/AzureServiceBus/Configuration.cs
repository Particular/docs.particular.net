using NServiceBus;

class ConfigureAzureServiceBusTransport
{
    public void Demo()
    {
        #region AzureServiceBusTransportWithAzure 6

        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.UseTransport<AzureServiceBusTransport>();

        #endregion
    }
}