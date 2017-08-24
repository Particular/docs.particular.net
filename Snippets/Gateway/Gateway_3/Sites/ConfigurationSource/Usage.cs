    using NServiceBus;

namespace Gateway_2.Sites.ConfigurationSource
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