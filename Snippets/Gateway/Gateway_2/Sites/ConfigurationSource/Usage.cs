using NServiceBus;

namespace Sites.ConfigurationSource
{
    class Usage
    {
        Usage(EndpointConfiguration endpointConfiguration)
        {
            #region UseCustomConfigurationSourceForGatewaySitesConfig

            endpointConfiguration.CustomConfigurationSource(new ConfigurationSource());

            #endregion
        }
    }
}