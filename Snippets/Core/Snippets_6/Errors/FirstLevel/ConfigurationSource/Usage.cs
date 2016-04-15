namespace Snippets6.Errors.FirstLevel.ConfigurationSource
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