namespace Snippets6.Gateway.Channels.ConfigurationSource
{
    using NServiceBus;

    public class Usage 
    {
        public Usage()
        {
            EndpointConfiguration configuration = new EndpointConfiguration();

            #region UseCustomConfigurationSourceForGatewayChannelsConfig
            configuration.CustomConfigurationSource(new ConfigurationSource());
            #endregion
        }
    }
}