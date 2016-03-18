namespace Snippets6.Gateway.Channels.ConfigurationSource
{
    using NServiceBus;

    public class Usage 
    {
        public Usage(EndpointConfiguration endpointConfiguration)
        {
            #region UseCustomConfigurationSourceForGatewayChannelsConfig
            endpointConfiguration.CustomConfigurationSource(new ConfigurationSource());
            #endregion
        }
    }
}