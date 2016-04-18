using NServiceBus;

namespace Core4.Sites.ConfigurationSource
{
    class ConfigurationSourceUsage
    {
        ConfigurationSourceUsage(Configure configure)
        {
            #region UseCustomConfigurationSourceForGatewaySitesConfig

            configure.CustomConfigurationSource(new ConfigurationSource());

            #endregion
        }
    }
}
