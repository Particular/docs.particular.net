using NServiceBus;
using NServiceBus.Gateway;

class Usage
{
    public void InMemoryDeduplicationConfigurationCacheSize(EndpointConfiguration endpointConfiguration)
    {
        #region NonDurableDeduplicationConfigurationCacheSize

        var gatewayStorageConfiguration = new InMemoryDeduplicationConfiguration
        {
            CacheSize = 50000,
        };

        endpointConfiguration.Gateway(gatewayStorageConfiguration);

        #endregion
    }
}
