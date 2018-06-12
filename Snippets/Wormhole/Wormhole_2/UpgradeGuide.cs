using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Router;
using NServiceBus.Settings;
using NServiceBus.Transport;

public class UpgradeGuide
{
    public async Task EndpointSide()
    {
        #region wormhole-to-router-connector

        var endpointConfiguration = new EndpointConfiguration("MyEndpoint");
        var transport = endpointConfiguration.UseTransport<LearningTransport>();
        var routing = transport.Routing();

        var router = routing.ConnectToRouter("SiteA");

        router.RouteToEndpoint(typeof(MyMessage), "Receiver");

        //Send a message

        var endpoint = await Endpoint.Start(endpointConfiguration);
        var options = new SendOptions();
        options.SendToSites("B", "C");

        await endpoint.Send(new MyMessage(), options);

        #endregion
    }

    public void RouterSide()
    {
        #region wormhole-to-router-router

        var routerConfig = new RouterConfiguration("SiteA");
        routerConfig.AddInterface<MsmqTransport>("Endpoints", tx => { });
        routerConfig.AddInterface<AzureStorageQueuesTransport>("Tunnel", tx => { });

        var staticRouting = routerConfig.UseStaticRoutingProtocol();
        staticRouting.AddRoute((iface, dest) => iface == "Endpoints", "From the local site", "SiteB", "Tunnel");
        staticRouting.AddRoute((iface, dest) => iface == "Tunnel", "From the remote site", null, "Endpoints");

        var router = Router.Create(routerConfig);

        #endregion
    }

    class MyMessage : IMessage
    {
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