namespace Snippets3.Routing.EndpointMapping.ConfigurationSource
{
    using NServiceBus;

    public class Usage
    {
        public Usage()
        {
            #region inject-endpoint-mapping-configuration-source

            Configure configure = Configure.With();
            configure.CustomConfigurationSource(new ConfigurationSource());

            #endregion
        }
    }
}