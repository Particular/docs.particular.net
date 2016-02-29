namespace Snippets6.Gateway.Sites.ConfigurationSource
{
    using NServiceBus;

    public class Usage 
    {
        public Usage()
        {
            EndpointConfiguration endpointConfiguration = new EndpointConfiguration();

            #region UseCustomConfigurationSourceForGatewaySitesConfig
            endpointConfiguration.CustomConfigurationSource(new ConfigurationSource());
            #endregion
        }
    }
}