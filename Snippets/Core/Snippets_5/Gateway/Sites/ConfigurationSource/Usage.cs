namespace Snippets5.Gateway.Sites.ConfigurationSource
{
    using NServiceBus;

    class Usage 
    {
        Usage(BusConfiguration busConfiguration)
        {
            #region UseCustomConfigurationSourceForGatewaySitesConfig
            busConfiguration.CustomConfigurationSource(new ConfigurationSource());
            #endregion
        }
    }
}