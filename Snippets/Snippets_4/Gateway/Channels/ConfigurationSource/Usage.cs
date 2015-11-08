namespace Snippets4.Gateway.Channels.ConfigurationSource
{
    using NServiceBus;

    public class Usage 
    {
        public Usage()
        {
            Configure configure = Configure.With();

            #region UseCustomConfigurationSourceForGatewayChannelsConfig
            configure.CustomConfigurationSource(new ConfigurationSource());
            #endregion
        }
    }
}