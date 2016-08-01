namespace Core6.Recoverability.Immediate.ConfigurationSource
{
    using NServiceBus;

    class Usage
    {
        Usage(EndpointConfiguration endpointConfiguration)
        {
            #region FLRConfigurationSourceUsage
            endpointConfiguration.CustomConfigurationSource(new ConfigurationSource());
            #endregion
        }
    }
}