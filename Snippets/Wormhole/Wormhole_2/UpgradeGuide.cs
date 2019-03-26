using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Router;
using NServiceBus.Routing;
using NServiceBus.Settings;
using NServiceBus.Transport;
using NServiceBus.Wormhole;
using NServiceBus.Wormhole.Gateway;
using SettingsHolder = NServiceBus.Settings.SettingsHolder;

public class UpgradeGuide
{
    public void WormholeConnector(EndpointConfiguration endpointConfiguration)
    {
        #region upgrade-wormhole-connector

        var routing = endpointConfiguration.UseWormholeGateway("Gateway.SiteA");
        routing.RouteToSite<MyMessage>(m => m.DestinationSite);

        #endregion
    }

    public async Task WormholeSend(IEndpointInstance endpointInstance)
    {
        #region upgrade-wormhole-connector-send

        await endpointInstance.Send(new MyMessage
        {
            DestinationSite = "SiteB"
        });

        #endregion
    }

    public void RouterConnector(EndpointConfiguration endpointConfiguration)
    {
        #region upgrade-router-connector

        var transport = endpointConfiguration.UseTransport<LearningTransport>();
        var routing = transport.Routing();

        var router = routing.ConnectToRouter("Gateway.SiteA");
        router.RouteToEndpoint(typeof(MyMessage), "EndpointB");

        #endregion
    }

    public async Task RouterSend(IEndpointInstance endpointInstance)
    {
        #region upgrade-router-connector-send

        var options = new SendOptions();
        options.SendToSites("SiteB");

        await endpointInstance.Send(new MyMessage(), options);

        #endregion
    }

    public void GatewaySideA()
    {
        #region upgrade-gateway-config-a

        var gatewayConfig = new WormholeGatewayConfiguration
            <MsmqTransport, AzureStorageQueuesTransport>("Gateway.SiteA", "SiteA");

        gatewayConfig.ConfigureRemoteSite("SiteB", "Gateway.SiteB");
        
        #endregion
    }

    public void GatewaySideB()
    {
        #region upgrade-gateway-config-b

        var gatewayConfig = new WormholeGatewayConfiguration
            <MsmqTransport, AzureStorageQueuesTransport>("Gateway.SiteB", "SiteB");

        gatewayConfig.ForwardToEndpoint(MessageType.Parse("MyMessage, Messages"), "EndpointB");

        #endregion
    }

    public void RouterSide()
    {
        #region upgrade-router-config

        var routerConfig = new RouterConfiguration("Gateway.SiteA");

        routerConfig.AddInterface<MsmqTransport>("Local", tx => { });
        routerConfig.AddInterface<AzureStorageQueuesTransport>("Tunnel", tx => { });

        var staticRouting = routerConfig.UseStaticRoutingProtocol();

        staticRouting.AddRoute((iface, dest) => iface == "Local", 
            "From the local site", "Gateway.SiteB", "Tunnel");

        staticRouting.AddRoute((iface, dest) => iface == "Tunnel", 
            "From the remote site", null, "Local");

        var router = Router.Create(routerConfig);

        #endregion
    }
    
    class MyMessage : IMessage
    {
        public string DestinationSite { get; set; }
    }

    public class AzureStorageQueuesTransport : TransportDefinition
    {
        public override TransportInfrastructure Initialize(SettingsHolder settings, string connectionString)
        {
            throw new NotImplementedException();
        }

        public override string ExampleConnectionStringForErrorMessage { get; }
    }

    public class MsmqTransport : TransportDefinition
    {
        public override TransportInfrastructure Initialize(SettingsHolder settings, string connectionString)
        {
            throw new NotImplementedException();
        }

        public override string ExampleConnectionStringForErrorMessage { get; }
    }
}