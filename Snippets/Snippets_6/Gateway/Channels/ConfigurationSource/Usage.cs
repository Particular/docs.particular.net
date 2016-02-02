namespace Snippets5.Gateway.Channels.ConfigurationSource
{
    using NServiceBus;

    public class Usage 
    {
        public Usage()
        {
            BusConfiguration busConfiguration = new BusConfiguration();

            #region UseCustomConfigurationSourceForGatewayChannelsConfig
            busConfiguration.CustomConfigurationSource(new ConfigurationSource());
            #endregion
        }
    }
}