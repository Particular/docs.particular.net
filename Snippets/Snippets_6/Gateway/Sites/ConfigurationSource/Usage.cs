namespace Snippets6.Gateway.Sites.ConfigurationSource
{
    using NServiceBus;

    public class Usage 
    {
        public Usage(EndpointConfiguration endpointConfiguration)
        {
            #region UseCustomConfigurationSourceForGatewaySitesConfig
            endpointConfiguration.CustomConfigurationSource(new ConfigurationSource());
            #endregion
        }
    }
}