using NServiceBus;

class Usage
{
    Usage(EndpointConfiguration endpointConfiguration)
    {
        #region AzureConfigurationSource

        endpointConfiguration.AzureConfigurationSource();

        #endregion      
    }
}