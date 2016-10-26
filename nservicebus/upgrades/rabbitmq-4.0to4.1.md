---
title: RabbitMQ Transport Upgrade Version 4.0 to 4.1
summary: Instructions on how to upgrade RabbitMQ Transport Version 4.0 to 4.1.
reviewed: 2016-10-26
tags:
 - upgrade
 - migration
related:
 - nservicebus/rabbitmq
 - nservicebus/rabbitmq/routing-topology
---


## Routing topology

### [Custom Routing Topology](/nservicebus/rabbitmq/routing-topology.md#custom-routing-topology)

The `UseRoutingTopology` method had an overload added with a parameter representing a factory delegate which creates the custom routing topology. This allows the use of non-default constructors for custom routing topology. The `UseRoutingTopology` method overload with no parameters was deprecated.

Change

snippet:rabbitmq-config-useroutingtopology4_0

to

snippet:rabbitmq-config-useroutingtopology4_0to4_1
