namespace Snippets6.Errors.FirstLevel.ConfigurationSource
{
    using NServiceBus;

    public class Usage
    {
        public Usage()
        {
            EndpointConfiguration endpointConfiguration = new EndpointConfiguration();

            #region FLRConfigurationSourceUsage
            endpointConfiguration.CustomConfigurationSource(new ConfigurationSource());
            #endregion
        }
    }
}