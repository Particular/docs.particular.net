namespace Core6.Recoverability.Delayed.ConfigurationSource
{
    using NServiceBus;

    class Usage
    {
        Usage(EndpointConfiguration endpointConfiguration)
        {
            #region SLRConfigurationSourceUsage
            endpointConfiguration.CustomConfigurationSource(new ConfigurationSource());
            #endregion
        }
    }
}