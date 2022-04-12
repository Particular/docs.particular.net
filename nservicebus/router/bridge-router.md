---
title: Upgrade Bridge 2 to Router 2
summary: Instructions on how to upgrade NServiceBus.Bridge Version 2 to NServiceBus.Router Version 2.
component: Router
reviewed: 2020-03-16
redirects:
- nservicebus/bridge/bridge-router
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 7
---

The `NServiceBus.Bridge` package has been deprecated and replaced by the more powerful `NServiceBus.Router` package. The bridge-based and switch-and-port-based APIs available in the Bridge package have been replaced by a single router-interface-based API.

## Endpoint-side

The main change on the endpoint side is the use of `ConnectToRouter` instead of `ConnectToBridge`. As the switch model has been removed, the port configuration is no longer available.

```c#
var endpointConfiguration = new EndpointConfiguration("MyEndpoint");
var transport = endpointConfiguration.UseTransport<LearningTransport>();
var routing = transport.Routing();

var router = routing.ConnectToRouter("MyRouter");

router.RouteToEndpoint(typeof(MyMessage), "Receiver");
router.RegisterPublisher(typeof(MyEvent), "Publisher");
```

NOTE: When connecting to the router, use the name of router's interface, not the name of the router itself.


## Bridge to router

A simple bridge between two transports can be constructed using the `Bridge.Between<TLeft>(...).And<TRight>(...)` construct. This API is no longer available. The following code is an equivalent using the router API.

```c#
var routerConfig = new RouterConfiguration("MyRouter");
routerConfig.AddInterface<MsmqTransport>("Left", extensions => { });
routerConfig.AddInterface<RabbitMQTransport>("Right",
    extensions => extensions.ConnectionString("host=localhost"));

var staticRouting = routerConfig.UseStaticRoutingProtocol();
staticRouting.AddForwardRoute("Left", "Right");
staticRouting.AddForwardRoute("Right", "Left");

var router = Router.Create(routerConfig);
```

A two-interface router is equivalent to a bridge but because the router can have any number of interfaces, it can no longer assume that messages should be forwarded to `Left` to `Right` and vice versa. The static routing protocol is used to configure the forwarding explicitly.

## Switch to router

A switch could be used to forward messages between more than two transports/brokers. The following code sets up a switch between sites A, B, and C.

```c#
var switchConfig = new SwitchConfiguration();
switchConfig.AddPort<RabbitMQTransport>("A", tx => tx.ConnectionString("host=a"));
switchConfig.AddPort<RabbitMQTransport>("B", tx => tx.ConnectionString("host=b"));
switchConfig.AddPort<RabbitMQTransport>("C", tx => tx.ConnectionString("host=c"));

switchConfig.PortTable["MyEndpoint"] = "A";
switchConfig.PortTable["OtherEndpoint"] = "C";

var @switch = Switch.Create(switchConfig);
```

NOTE: When using a switch, endpoints must be explicitly mapped to ports, either in the bridge connector or in the bridge itself.

The replacement router configuration is similar but uses interfaces instead of ports. The router interface mapping is more powerful than switch port mapping and allows for complex routing rules based on both the incoming interface and the destination. The result is the outgoing interface and, optionally, a next hop router (also known as a [gateway](/nservicebus/gateway)).

```c#
var routerConfig = new RouterConfiguration("MyRouter");
routerConfig.AddInterface<RabbitMQTransport>("A", tx => tx.ConnectionString("host=a"));
routerConfig.AddInterface<RabbitMQTransport>("B", tx => tx.ConnectionString("host=b"));
routerConfig.AddInterface<RabbitMQTransport>("C", tx => tx.ConnectionString("host=c"));

var staticRouting = routerConfig.UseStaticRoutingProtocol();
staticRouting.AddRoute((iface, dest) => dest.Endpoint == "MyEndpoint", "To MyEndpoint", null, "A");
staticRouting.AddRoute((iface, dest) => dest.Endpoint == "OtherEndpoint", "To OtherEndpoint", null, "C");

var router = Router.Create(routerConfig);
```

