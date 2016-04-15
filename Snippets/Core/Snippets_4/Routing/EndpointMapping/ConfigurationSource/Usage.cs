namespace Core4.Routing.EndpointMapping.ConfigurationSource
{
    using NServiceBus;

    class Usage
    {
        Usage(Configure configure)
        {
            #region inject-endpoint-mapping-configuration-source

            configure.CustomConfigurationSource(new ConfigurationSource());

            #endregion
        }
    }
}