namespace Snippets5.Gateway.Sites.ConfigurationSource
{
    using NServiceBus;

    public class Usage 
    {
        public Usage()
        {
            EndpointConfiguration configuration = new EndpointConfiguration();

            #region UseCustomConfigurationSourceForGatewaySitesConfig
            configuration.CustomConfigurationSource(new ConfigurationSource());
            #endregion
        }
    }
}