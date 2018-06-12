using System;
using NServiceBus;
using NServiceBus.Router;
using NServiceBus.Settings;
using NServiceBus.Transport;
using NServiceBus.Bridge;

public class UpgradeGuide
{
    public void Connector()
    {
        #region bridge-to-router-connector

        var endpointConfiguration = new EndpointConfiguration("MyEndpoint");
        var transport = endpointConfiguration.UseTransport<LearningTransport>();
        var routing = transport.Routing();
        
        var router = routing.ConnectToRouter("MyRouter");

        router.RouteToEndpoint(typeof(MyMessage), "Receiver");
        router.RegisterPublisher(typeof(MyEvent), "Publisher");

        #endregion
    }

    public void SimpleRouter()
    {
        #region bridge-to-router-simple-router

        var routerConfig = new RouterConfiguration("MyRouter");
        routerConfig.AddInterface<MsmqTransport>("Left", extensions => { });
        routerConfig.AddInterface<RabbitMQTransport>("Right",
            extensions => extensions.ConnectionString("host=localhost"));

        var staticRouting = routerConfig.UseStaticRoutingProtocol();
        staticRouting.AddForwardRoute("Left", "Right");
        staticRouting.AddForwardRoute("Right", "Left");

        var router = Router.Create(routerConfig);

        #endregion
    }

    public void ThreeWaySwitch()
    {
        #region bridge-to-router-switch

        var switchConfig = new SwitchConfiguration();
        switchConfig.AddPort<RabbitMQTransport>("A", tx => tx.ConnectionString("host=a"));
        switchConfig.AddPort<RabbitMQTransport>("B", tx => tx.ConnectionString("host=b"));
        switchConfig.AddPort<RabbitMQTransport>("C", tx => tx.ConnectionString("host=c"));

        switchConfig.PortTable["MyEndpoint"] = "A";
        switchConfig.PortTable["OtherEndpoint"] = "C";

        var @switch = Switch.Create(switchConfig);

        #endregion
    }

    public void ThreeWayRouter()
    {
        #region bridge-to-router-three-way-router

        var routerConfig = new RouterConfiguration("MyRouter");
        routerConfig.AddInterface<RabbitMQTransport>("A", tx => tx.ConnectionString("host=a"));
        routerConfig.AddInterface<RabbitMQTransport>("B", tx => tx.ConnectionString("host=b"));
        routerConfig.AddInterface<RabbitMQTransport>("C", tx => tx.ConnectionString("host=c"));

        var staticRouting = routerConfig.UseStaticRoutingProtocol();
        staticRouting.AddRoute((iface, dest) => dest.Endpoint == "MyEndpoint", "To MyEndpoint", null, "A");
        staticRouting.AddRoute((iface, dest) => dest.Endpoint == "OtherEndpoint", "To OtherEndpoint", null, "C");

        var router = Router.Create(routerConfig);

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

    public class MsmqTransport : TransportDefinition
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