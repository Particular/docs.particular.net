namespace Snippets6.Routing.EndpointMapping.ConfigurationSource
{
    using NServiceBus;

    public class Usage 
    {
        public Usage()
        {
            EndpointConfiguration configuration = new EndpointConfiguration();
            #region inject-endpoint-mapping-configuration-source
            configuration.CustomConfigurationSource(new ConfigurationSource());
            #endregion
        }
    }
}