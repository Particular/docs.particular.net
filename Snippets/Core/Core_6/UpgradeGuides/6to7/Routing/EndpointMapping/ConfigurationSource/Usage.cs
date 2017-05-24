namespace Core_6.UpgradeGuides._6to7.Routing.EndpointMapping.ConfigurationSource
{
    using NServiceBus;

    class Usage
    {
        Usage(EndpointConfiguration endpointConfiguration)
        {
            #region 6to7inject-endpoint-mapping-configuration-source
            endpointConfiguration.CustomConfigurationSource(new ConfigurationSource());
            #endregion
        }
    }
}