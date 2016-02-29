namespace Snippets6.Gateway
{
    using NServiceBus;
    using NServiceBus.Features;

    public class Usage
    {
        public Usage()
        {
            #region GatewayConfiguration

            EndpointConfiguration endpointConfiguration = new EndpointConfiguration();
            endpointConfiguration.EnableFeature<Gateway>();

            #endregion

            IEndpointInstance endpoint = null;

            #region SendToSites

            endpoint.SendToSites(new[] { "SiteA", "SiteB" }, new MyMessage());

            #endregion
        }

    }
}
