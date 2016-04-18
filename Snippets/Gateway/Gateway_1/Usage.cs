namespace Gateway_1
{
    using NServiceBus;
    using NServiceBus.Features;

    class Usage
    {
        Usage(BusConfiguration busConfiguration, IBus Bus)
        {
            #region GatewayConfiguration

            busConfiguration.EnableFeature<Gateway>();

            #endregion

            #region SendToSites

            Bus.SendToSites(new[] { "SiteA", "SiteB" }, new MyMessage());

            #endregion
        }

    }
}
