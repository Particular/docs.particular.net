using NServiceBus;
using NServiceBus.Gateway.Channels.HttpVNext;

class Usage
{
    void ConfigureReceiveEndpoint(EndpointConfiguration endpointConfiguration)
    {
        #region HttpVNextChannel
        var gatewayConfig = endpointConfiguration.Gateway();
        gatewayConfig.ChannelFactories(s => new HttpVNextChannelSender(), r => new HttpVNextChannelReceiver());

        gatewayConfig.AddReceiveChannel("http://Headquarter.mycorp.com/", "httpVNext");
        #endregion
    }

    void ConfigureSiteEndpoint(EndpointConfiguration endpointConfiguration)
    {
        #region HttpVNextSite
        var gatewayConfig = endpointConfiguration.Gateway();
        gatewayConfig.ChannelFactories(s => new HttpVNextChannelSender(), r => new HttpVNextChannelReceiver());

        gatewayConfig.AddSite("SiteA", "http://SiteA.mycorp.com");
        #endregion
    }
}

