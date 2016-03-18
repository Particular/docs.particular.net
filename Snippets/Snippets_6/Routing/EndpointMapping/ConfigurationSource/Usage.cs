namespace Snippets6.Routing.EndpointMapping.ConfigurationSource
{
    using NServiceBus;

    public class Usage 
    {
        public Usage(EndpointConfiguration endpointConfiguration)
        {
            #region inject-endpoint-mapping-configuration-source
            endpointConfiguration.CustomConfigurationSource(new ConfigurationSource());
            #endregion
        }
    }
}