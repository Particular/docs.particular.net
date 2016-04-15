namespace Snippets4.Gateway.Channels.ConfigurationSource
{
    using NServiceBus;

    class Usage
    {
        Usage(Configure configure)
        {
            #region UseCustomConfigurationSourceForGatewayChannelsConfig

            configure.CustomConfigurationSource(new ConfigurationSource());

            #endregion
        }
    }
}