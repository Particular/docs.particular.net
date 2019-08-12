using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Router;
using NServiceBus.Settings;
using NServiceBus.Transport;

public class API
{
    public void Connector()
    {
        #region connector

        var endpointConfiguration = new EndpointConfiguration("MyEndpoint");
        var transport = endpointConfiguration.UseTransport<MsmqTransport>();
        var routing = transport.Routing();
        var bridge = routing.ConnectToRouter("MyRouter");

        //routing.RouteToEndpoint(ty);
        bridge.RouteToEndpoint(typeof(MyMessage), "Receiver");
        bridge.RegisterPublisher(typeof(MyEvent), "Publisher");

        #endregion
    }

    public async Task TwoWayRouterConfig()
    {
        #region two-way-router

        var routerConfig = new RouterConfiguration("MyRouter");

        routerConfig.AddInterface<MsmqTransport>("Msmq", 
            customization: transportExtensions => { });

        routerConfig.AddInterface<RabbitMQTransport>("Rabbit",
            customization: transportExtensions =>
            {
                transportExtensions.ConnectionString("host=localhost");
            });

        #endregion

        #region simple-routing

        var staticRouting = routerConfig.UseStaticRoutingProtocol();

        //Forward all messages from MSMQ to RabbitMQ
        staticRouting.AddForwardRoute(
            incomingInterface: "Msmq",
            outgoingInterface: "Rabbit");

        //Forward all messages from RabbitMQ to MSMQ
        staticRouting.AddForwardRoute(
            incomingInterface: "Rabbit",
            outgoingInterface: "Msmq");

        #endregion

        #region lifecycle

        var router = Router.Create(routerConfig);

        await router.Start().ConfigureAwait(false);

        await router.Stop().ConfigureAwait(false);

        #endregion
    }

    public void ThreeWayRouterConfig()
    {
        #region three-way-router

        var routerConfig = new RouterConfiguration("MyRouter");

        routerConfig.AddInterface<SqlTransport>("A", transportExtensions => {});
        routerConfig.AddInterface<SqlTransport>("B", transportExtensions => {});
        routerConfig.AddInterface<SqlTransport>("C", transportExtensions => {});

        var staticRouting = routerConfig.UseStaticRoutingProtocol();
        //Send all messages to endpoints which name starts with Sales via interface A
        staticRouting.AddRoute(
            destinationFilter: (iface, destination) => destination.Endpoint.StartsWith("Sales."),
            destinationFilterDescription: "To Sales", 
            gateway: null, 
            iface: "A");

        staticRouting.AddRoute(
            (iface, destination) => destination.Endpoint.StartsWith("Shipping."), 
            "To Shipping", null, "B");

        staticRouting.AddRoute(
            (iface, destination) => destination.Endpoint.StartsWith("Billing."),
            "To Billing", null, "C");

        #endregion
    }

    public void SiteBasedRouting()
    {
        #region backplane

        var routerConfig = new RouterConfiguration("Router.WestEurope");

        routerConfig.AddInterface<MsmqTransport>("Endpoints", transportExtensions => { });
        routerConfig.AddInterface<AzureStorageQueuesTransport>("Backplane", transportExtensions => { });

        var staticRouting = routerConfig.UseStaticRoutingProtocol();

        //Send all messages from the Backplane interface directly to the destination endpoints
        staticRouting.AddRoute(
            (iface, destination) => iface == "Backplane",
            "From outside", null, "Endpoints");

        //Send all messages to site WestUS through the Backplane interface via Router.WestUS
        staticRouting.AddRoute(
            destinationFilter: (iface, destination) => destination.Site == "WestUS",
            destinationFilterDescription: "To West US",
            gateway: "Router.WestUS",
            iface: "Backplane");

        //Send all messages to site EastUS through the Backplane interface via Router.EastUS
        staticRouting.AddRoute(
            (iface, destination) => destination.Site == "EastUS",
            "To East US", "Router.EastUS", "Backplane");

        #endregion
    }

    public void OtherAPIs()
    {
        var routerConfig = new RouterConfiguration("MyRouter");

        #region recoverability

        routerConfig.CircuitBreakerThreshold = 20;
        routerConfig.DelayedRetries = 10;
        routerConfig.ImmediateRetries = 10;

        #endregion

        #region queue-creation

        routerConfig.AutoCreateQueues(identity: "Bob");

        #endregion
    }

    public class RabbitMQTransport : TransportDefinition
    {
        public override TransportInfrastructure Initialize(SettingsHolder settings, string connectionString)
        {
            throw new NotImplementedException();
        }

        public override string ExampleConnectionStringForErrorMessage { get; }
    }

    public class SqlTransport : TransportDefinition
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

    public class AzureStorageQueuesTransport : TransportDefinition
    {
        public override TransportInfrastructure Initialize(SettingsHolder settings, string connectionString)
        {
            throw new NotImplementedException();
        }

        public override string ExampleConnectionStringForErrorMessage { get; }
    }

    public class MyEvent
    {
    }

    public class MyMessage
    {
    }
}
