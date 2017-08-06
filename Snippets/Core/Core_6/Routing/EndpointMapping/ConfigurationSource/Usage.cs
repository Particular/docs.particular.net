namespace Core6.Routing.EndpointMapping.ConfigurationSource
{
    using NServiceBus;

    class Usage
    {
        Usage(EndpointConfiguration endpointConfiguration)
        {
            #region inject-endpoint-mapping-configuration-source
            endpointConfiguration.CustomConfigurationSource(new ConfigurationSource());
            #endregion
        }
    }
}