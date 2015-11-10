namespace Snippets5.Gateway
{
    using NServiceBus;
    using NServiceBus.Features;

    public class Usage
    {
        public Usage()
        {
            #region GatewayConfiguration

            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.EnableFeature<Gateway>();

            #endregion

            IBus Bus = null;

            #region SendToSites

            Bus.SendToSites(new[] { "SiteA", "SiteB" }, new MyMessage());

            #endregion
        }

    }
}
