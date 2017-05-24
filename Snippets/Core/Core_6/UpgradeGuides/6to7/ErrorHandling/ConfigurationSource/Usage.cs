namespace Core_6.UpgradeGuides._6to7.ErrorHandling.ConfigurationSource
{
    using NServiceBus;

    class Usage
    {
        Usage(EndpointConfiguration endpointConfiguration)
        {
            #region 6to7UseCustomConfigurationSourceForErrorQueueConfig
            endpointConfiguration.CustomConfigurationSource(new ConfigurationSource());
            #endregion
        }
    }
}