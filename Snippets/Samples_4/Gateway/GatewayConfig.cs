using NServiceBus;
using NServiceBus.Features;

public class GatewayConfig
{
    public GatewayConfig()
    {
        #region GatewayConfiguration-v4

        Configure.Instance.RunGateway();

        #endregion

        IBus Bus = null;

        #region GatewayApi-v5

        Bus.SendToSites(new[] { "SiteA", "SiteB" }, new MyCrossSiteMessage());

        #endregion

    }

    public class MyCrossSiteMessage
    {
    }
}


