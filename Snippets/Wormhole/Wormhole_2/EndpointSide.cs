using NServiceBus;
using System.Collections.Generic;
using NServiceBus.Routing;
using NServiceBus.Wormhole;
using NServiceBus.Wormhole.Gateway;

public class EndpointSide
{
    public void Configure(EndpointConfiguration endpointConfiguration)
    {
        #region DestinationSites

        var routing = endpointConfiguration.UseWormholeGateway("Gateway.SiteA");

        routing.RouteToSite<MyMessage>("SiteB");
        routing.RouteToSite<OtherMessage>(m => m.Destination);

        #endregion
    }

    public void ConfigureGateways(
        WormholeGatewayConfiguration<LearningTransport, LearningTransport> configSiteA,
        WormholeGatewayConfiguration<LearningTransport, LearningTransport> configSiteB)
    {
        #region RemoteSiteA

        configSiteA.ConfigureRemoteSite("SiteB", "Gateway.SiteB");

        #endregion

        #region RemoteSiteB

        configSiteB.ConfigureRemoteSite("SiteA", "Gateway.SiteA");

        #endregion
    }

    public void ConfigureDestinationGateway(WormholeGatewayConfiguration<MsmqTransport, MsmqTransport> config)
    {
        #region DestinationEndpoints

        config.ForwardToEndpoint(MessageType.Parse("MyMessage, Messages"), "EndpointA");
        config.ForwardToEndpoint("OtherMessages", "SomeNamespace", "EndpointB");
        config.ForwardToEndpoint("OtherMessages", "EndpointB");

        #endregion

        #region SSD

        config.EndpointInstances.AddOrReplaceInstances(
            sourceKey: "SSD",
            endpointInstances: new List<EndpointInstance>
            {
                new EndpointInstance(endpoint: "ScaledOutEndpoint").AtMachine("Host1"),
                new EndpointInstance(endpoint: "ScaledOutEndpoint").AtMachine("Host2")
            });

        #endregion
    }

    class MyMessage : IMessage
    {
    }

    class OtherMessage : IMessage
    {
        public string Destination { get; set; }
    }
}