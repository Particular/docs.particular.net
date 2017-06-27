---
title: RabbitMQ Transport Upgrade Version 4.0 to 4.1
summary: Routing topology Changes in Version 4.0 to 4.1.
reviewed: 2016-10-26
component: Rabbit
related:
 - transports/rabbitmq/routing-topology
redirects:
 - nservicebus/upgrades/rabbitmq-4.0to4.1
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 6
---


## [Custom Routing Topology](/transports/rabbitmq/routing-topology.md#custom-routing-topology)

The `UseRoutingTopology` method has a new overload with a parameter representing a factory delegate which creates the custom routing topology. This allows the use of non-default constructors for custom routing topology instances. The `UseRoutingTopology` method overload with no parameters is deprecated.

snippet: 40to41rabbitmq-useroutingtopology
