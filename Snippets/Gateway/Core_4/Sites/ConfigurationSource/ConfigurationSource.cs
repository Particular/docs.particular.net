using System.Configuration;
using NServiceBus.Config;
using NServiceBus.Config.ConfigurationSource;


namespace Core4.Sites.ConfigurationSource
{
    #region GatewaySitesConfigurationSource

    public class ConfigurationSource :
        IConfigurationSource
    {
        public T GetConfiguration<T>() where T : class, new()
        {
            if (typeof(T) == typeof(GatewayConfig))
            {
                var gatewayConfig = new GatewayConfig
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

            // Respect app.config for other sections not defined in this method
            return ConfigurationManager.GetSection(typeof(T).Name) as T;
        }
    }

    #endregion
}