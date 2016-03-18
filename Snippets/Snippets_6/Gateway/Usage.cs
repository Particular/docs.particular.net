namespace Snippets6.Gateway
{
    using NServiceBus;
    using NServiceBus.Features;

    public class Usage
    {
        public Usage(EndpointConfiguration endpointConfiguration)
        {
            #region GatewayConfiguration
            endpointConfiguration.EnableFeature<Gateway>();

            #endregion

            IEndpointInstance endpoint = null;

            #region SendToSites

            endpoint.SendToSites(new[] { "SiteA", "SiteB" }, new MyMessage());

            #endregion
        }

    }
}
