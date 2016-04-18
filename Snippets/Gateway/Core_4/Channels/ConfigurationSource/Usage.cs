namespace Core4.Channels.ConfigurationSource
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