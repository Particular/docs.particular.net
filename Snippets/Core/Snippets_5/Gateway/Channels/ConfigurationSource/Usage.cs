namespace Core5.Gateway.Channels.ConfigurationSource
{
    using NServiceBus;

    class Usage 
    {
        Usage(BusConfiguration busConfiguration)
        {
            #region UseCustomConfigurationSourceForGatewayChannelsConfig
            busConfiguration.CustomConfigurationSource(new ConfigurationSource());
            #endregion
        }
    }
}