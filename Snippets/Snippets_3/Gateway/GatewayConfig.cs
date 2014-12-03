using NServiceBus;

public class GatewayConfig
{
    public GatewayConfig()
    {
        #region GatewayConfiguration 3

        Configure.Instance.RunGateway();

        #endregion

        IBus Bus = null;

        #region SendToSites 3

        Bus.SendToSites(new[] { "SiteA", "SiteB" }, new MyCrossSiteMessage());

        #endregion

    }

    public class MyCrossSiteMessage
    {
    }
}


