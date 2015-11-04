using NServiceBus.Config;
using NServiceBus.Config.ConfigurationSource;

#region HeadquatersGatewayCodeConfig
class GatewayConfigConfigurationProvider : IProvideConfiguration<GatewayConfig>
{
    public GatewayConfig GetConfiguration()
    {
        return new GatewayConfig
        {
            Channels =
            {
                new ChannelConfig
                {
                    Address = "http://localhost:25899/Headquarters/",
                    ChannelType = "Http"
                }
            },
            Sites =
            {
                new SiteConfig
                {
                    Address = "http://localhost:25899/RemoteSite/",
                    ChannelType = "Http",
                    Key = "RemoteSite"
                }
            }
        };
    }
}
#endregion
