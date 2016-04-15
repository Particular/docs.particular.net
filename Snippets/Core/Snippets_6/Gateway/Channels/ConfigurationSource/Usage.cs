namespace Snippets6.Gateway.Channels.ConfigurationSource
{
    using NServiceBus;

    class Usage 
    {
        Usage(EndpointConfiguration endpointConfiguration)
        {
            #region UseCustomConfigurationSourceForGatewayChannelsConfig
            endpointConfiguration.CustomConfigurationSource(new ConfigurationSource());
            #endregion
        }
    }
}