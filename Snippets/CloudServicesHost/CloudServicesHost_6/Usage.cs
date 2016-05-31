using NServiceBus;

class Usage
{
    Usage(BusConfiguration busConfiguration)
    {
        #region AzureConfigurationSource

        busConfiguration.AzureConfigurationSource();

        #endregion
    }
}