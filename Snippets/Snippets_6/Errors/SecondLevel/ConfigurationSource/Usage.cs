namespace Snippets6.Errors.SecondLevel.ConfigurationSource
{
    using NServiceBus;

    public class Usage
    {
        public Usage()
        {
            EndpointConfiguration endpointConfiguration = new EndpointConfiguration();

            #region SLRConfigurationSourceUsage
            endpointConfiguration.CustomConfigurationSource(new ConfigurationSource());
            #endregion
        }
    }
}