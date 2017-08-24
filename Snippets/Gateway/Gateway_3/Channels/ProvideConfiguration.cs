namespace Gateway_2.Channels
{
    using NServiceBus.Config;
    using NServiceBus.Config.ConfigurationSource;

    #region GatewayChannelsConfigurationProvider

    class ProvideConfiguration :
        IProvideConfiguration<GatewayConfig>
    {
        public GatewayConfig GetConfiguration()
        {
            return new GatewayConfig
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
        }
    }

    #endregion
}