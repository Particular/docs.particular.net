using NServiceBus.Config;
using NServiceBus.Config.ConfigurationSource;

#region GatewaySitesConfigurationProvider

namespace Core4.Sites
{
    class ProvideConfiguration : IProvideConfiguration<GatewayConfig>
    {
        public GatewayConfig GetConfiguration()
        {
            return new GatewayConfig
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
        }
    }
}

#endregion
