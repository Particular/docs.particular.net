namespace Snippets6.Gateway.Channels.ConfigurationSource
{
    using NServiceBus;

    public class Usage 
    {
        public Usage()
        {
            EndpointConfiguration endpointConfiguration = new EndpointConfiguration();

            #region UseCustomConfigurationSourceForGatewayChannelsConfig
            endpointConfiguration.CustomConfigurationSource(new ConfigurationSource());
            #endregion
        }
    }
}