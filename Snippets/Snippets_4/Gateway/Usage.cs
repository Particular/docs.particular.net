namespace Snippets4.Gateway
{
    using NServiceBus;

    public class Usage
    {
        public Usage()
        {
            #region GatewayConfiguration

            Configure.Instance.RunGateway();

            #endregion

            IBus Bus = null;

            #region SendToSites

            Bus.SendToSites(new[] { "SiteA", "SiteB" }, new MyMessage());

            #endregion

        }

    }
}