using NServiceBus;

public class GatewayConfig
{
    public GatewayConfig()
    {
        #region GatewayConfiguration 4

        Configure.Instance.RunGateway();

        #endregion

        IBus Bus = null;

        #region SendToSites 4

        Bus.SendToSites(new[] { "SiteA", "SiteB" }, new MyCrossSiteMessage());

        #endregion

    }

    public class MyCrossSiteMessage
    {
    }
}


