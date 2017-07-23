namespace Core5.Routing.EndpointMapping.ConfigurationSource
{
    using NServiceBus;

    class Usage
    {
        Usage(BusConfiguration busConfiguration)
        {
            #region inject-endpoint-mapping-configuration-source
            busConfiguration.CustomConfigurationSource(new ConfigurationSource());
            #endregion
        }
    }
}