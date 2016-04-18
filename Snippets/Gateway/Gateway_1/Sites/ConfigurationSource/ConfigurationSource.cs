namespace Gateway_1.Sites.ConfigurationSource
{
    using System.Configuration;
    using NServiceBus.Config;
    using NServiceBus.Config.ConfigurationSource;

    #region GatewaySitesConfigurationSource

    public class ConfigurationSource : IConfigurationSource
    {
        public T GetConfiguration<T>() where T : class, new()
        {
            if (typeof(T) == typeof(GatewayConfig))
            {
                GatewayConfig gatewayConfig = new GatewayConfig
                {
                    Sites = new SiteCollection
                    {
                        new SiteConfig
                        {
                            Key = "SiteA",
                            Address = "http://SiteA.mycorp.com/",
                            ChannelType = "Http"
                        },
                        new SiteConfig
                        {
                            Key = "SiteB",
                            Address = "http://SiteB.mycorp.com/",
                            ChannelType = "Http"
                        }
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