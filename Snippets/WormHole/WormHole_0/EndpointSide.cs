using NServiceBus;
using NServiceBus.WormHole;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceBus.Routing;
using NServiceBus.WormHole.Gateway;

namespace WormHole_0
{
    public class EndpointSide
    {
        public void Configure(EndpointConfiguration config)
        {
            
            #region DestinationSites

            var routing = config.UseWormHoleGateway("Gateway.SiteA");

            routing.RouteToSite<MyMessage>("SiteB");
            routing.RouteToSite<OtherMessage>(m => m.Destination);

            #endregion

        }

        public void ConfigureGateways(
            WormHoleGatewayConfiguration<MsmqTransport, MsmqTransport> configSiteA,
            WormHoleGatewayConfiguration<MsmqTransport, MsmqTransport> configSiteB)
        {
            
            #region RemoteSiteA

            configSiteA.ConfigureRemoteSite("SiteB", "Gateway.SiteB");

            #endregion

            #region  RemoteSiteB

            configSiteB.ConfigureRemoteSite("SiteA", "Gateway.SiteA");

            #endregion

        }

        public void ConfigureDestinationGateway(WormHoleGatewayConfiguration<MsmqTransport, MsmqTransport> config)
        {

            #region DestinationEndpoints

            config.ForwardToEndpoint(MessageType.Parse("MyMessage, Messages"), "EndpointA");
            config.ForwardToEndpoint("OtherMessages", "SomeNamespace", "EndpointB");
            config.ForwardToEndpoint("OtherMessages", "EndpointB");

            #endregion

            #region SSD

            config.EndpointInstances.AddOrReplaceInstances("SSD",
                new List<EndpointInstance>
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
}
