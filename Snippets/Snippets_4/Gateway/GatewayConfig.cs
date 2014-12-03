using NServiceBus;

public class GatewayConfig
{
    public GatewayConfig()
    {
        #region GatewayConfiguration 4

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


