namespace Gateway_1.Sites.ConfigurationSource
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