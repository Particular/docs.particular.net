namespace Core4.Gateway.Sites.ConfigurationSource
{
    using NServiceBus;

    class Usage
    {
        Usage(Configure configure)
        {
            #region UseCustomConfigurationSourceForGatewaySitesConfig

            configure.CustomConfigurationSource(new ConfigurationSource());

            #endregion
        }
    }
}