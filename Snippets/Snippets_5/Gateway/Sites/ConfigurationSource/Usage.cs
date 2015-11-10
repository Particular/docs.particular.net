namespace Snippets5.Gateway.Sites.ConfigurationSource
{
    using NServiceBus;

    public class Usage 
    {
        public Usage()
        {
            BusConfiguration busConfiguration = new BusConfiguration();

            #region UseCustomConfigurationSourceForGatewaySitesConfig
            busConfiguration.CustomConfigurationSource(new ConfigurationSource());
            #endregion
        }
    }
}