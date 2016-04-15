namespace Core6.Gateway.Sites.ConfigurationSource
{
    using NServiceBus;

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