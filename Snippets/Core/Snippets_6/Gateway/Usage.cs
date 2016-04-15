namespace Core6.Gateway
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
