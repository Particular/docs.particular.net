namespace Gateway_2
{
    using NServiceBus;
    using NServiceBus.Features;

    class Usage
    {
        Usage(EndpointConfiguration endpointConfiguration, IEndpointInstance endpoint)
        {
            #region GatewayConfiguration
            endpointConfiguration.EnableFeature<Gateway>();

            #endregion

            #region SendToSites

            endpoint.SendToSites(new[] { "SiteA", "SiteB" }, new MyMessage());

            #endregion
        }

    }
}
