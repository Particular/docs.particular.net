using NServiceBus;
using NServiceBus.Features;

public class GatewayConfig
{
    public GatewayConfig()
    {
        #region GatewayConfiguration

        BusConfiguration configuration = new BusConfiguration();

        configuration.EnableFeature<Gateway>();

        #endregion

        IBus Bus = null;

        #region SendToSites

        Bus.SendToSites(new[] { "SiteA", "SiteB" }, new MyCrossSiteMessage());

        #endregion
    }
    public class MyCrossSiteMessage
    {
    }

}
