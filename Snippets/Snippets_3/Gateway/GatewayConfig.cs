using NServiceBus;

public class GatewayConfig
{
    public GatewayConfig()
    {
        #region GatewayConfiguration

        Configure.Instance.RunGateway();

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


