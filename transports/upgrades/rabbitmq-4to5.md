---
title: RabbitMQ Transport Upgrade Version 4 to 5
summary: Instructions on how to upgrade RabbitMQ Transport Version 4 to 5.
reviewed: 2017-09-07
component: Rabbit
related:
 - nservicebus/upgrades/6to7
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 6
 - 7
---

## Mandatory routing topology selection

Specifying a routing topology is now mandatory. For backwards compatibility, the recommendation is to use the `ConventionalRoutingTopology`, which was the previous default. 

See [routing topology](/transports/rabbitmq/routing-topology.md) for further details.


## Direct routing topology changes

The convention for overriding the name of the exchange used when publishing events has changed. The address and the event type are no longer passed to the `exchangeNameConvention` parameter of the [UseDirectRoutingTopology](/transports/rabbitmq/routing-topology.md#direct-routing-topology-enabling-direct-routing-topology) method.

snippet: 4to5usedirectroutingtopology


## Custom routing topology changes

The following changes have been made to the interfaces used to create custom routing topologies.


### IDeclareQueues removed

The [IDeclareQueues](/transports/rabbitmq/routing-topology.md?version=rabbit_4#custom-routing-topology-taking-control-of-queue-declaration) interface added in Version 4.2 has been removed. The two address parameters on the interface's `DeclareAndInitialize` method have been added to the `Initialize` method of the `IRoutingTopology` interface. Implementations of the `Initialize` method are now responsible for creating any queues required by the topology.


### ISupportDelayedDelivery removed

The [ISupportDelayedDelivery](/transports/rabbitmq/routing-topology.md?version=rabbit_4#custom-routing-topology-delayed-delivery) interface added in Version 4.3 has been removed. The `BindToDelayInfrastructure` method is now part of the `IRoutingTopology` interface.