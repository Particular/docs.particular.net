namespace Gateway_1.Channels.ConfigurationSource
{
    using System.Configuration;
    using NServiceBus.Config;
    using NServiceBus.Config.ConfigurationSource;

    #region GatewayChannelsConfigurationSource

    public class ConfigurationSource : IConfigurationSource
    {
        public T GetConfiguration<T>() where T : class, new()
        {
            if (typeof(T) == typeof(GatewayConfig))
            {
                GatewayConfig gatewayConfig = new GatewayConfig
                {
                    Channels = new ChannelCollection
                {
                    new ChannelConfig
                    {
                        Address = "http://Headquarter.mycorp.com/",
                        Default = true,
                        ChannelType = "Http"
                    },
                    new ChannelConfig
                    {
                        Address = "http://Headquarter.myotherdomain.com/",
                        ChannelType = "Http"
                    },
                }
                };

                return gatewayConfig as T;
            }

            // To in app.config for other sections not defined in this method, otherwise return null.
            return ConfigurationManager.GetSection(typeof(T).Name) as T;
        }
    }

    #endregion
}