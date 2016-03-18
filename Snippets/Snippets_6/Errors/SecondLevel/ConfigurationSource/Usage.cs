namespace Snippets6.Errors.SecondLevel.ConfigurationSource
{
    using NServiceBus;

    public class Usage
    {
        public Usage(EndpointConfiguration endpointConfiguration)
        {
            #region SLRConfigurationSourceUsage
            endpointConfiguration.CustomConfigurationSource(new ConfigurationSource());
            #endregion
        }
    }
}