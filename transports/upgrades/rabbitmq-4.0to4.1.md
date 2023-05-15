---
title: RabbitMQ Transport Upgrade Version 4.0 to 4.1
summary: Routing topology changes from RabbitMQ version 4.0 to 4.1.
reviewed: 2023-05-14
component: Rabbit
related:
 - transports/rabbitmq/routing-topology
redirects:
 - nservicebus/upgrades/rabbitmq-4.0to4.1
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 6
---


## [Custom routing topology](/transports/rabbitmq/routing-topology.md#custom-routing-topology)

The `UseRoutingTopology` method has a new overload with a parameter representing a factory delegate, which creates the custom routing topology. The factory delegate allows the use of non-default constructors for custom routing topology instances. The `UseRoutingTopology` method overload with no parameters is deprecated.

```csharp
// For RabbitMQ Transport version 4.x
var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
transport.UseRoutingTopology<MyRoutingTopology>();

// For RabbitMQ Transport version 4.x
var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
transport.UseRoutingTopology(
    topologyFactory: createDurableExchangesAndQueues =>
    {
        return new MyRoutingTopology();
    });
```
