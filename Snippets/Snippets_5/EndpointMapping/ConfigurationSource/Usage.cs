namespace Snippets5.EndpointMapping.ConfigurationSource
{
    using NServiceBus;

    public class Usage 
    {
        public Usage()
        {
            BusConfiguration busConfiguration = new BusConfiguration();
            #region inject-endpoint-mapping-configuration-source
            busConfiguration.CustomConfigurationSource(new ConfigurationSource());
            #endregion
        }
    }
}