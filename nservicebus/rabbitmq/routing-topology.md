---
title: RabbitMQ Transport Routing topology
reviewed: 2016-08-30
component: Rabbit
versions: '[2,]'
---


The RabbitMQ transport has the concept of a routing topology, which controls how it creates exchanges, queues, and the bindings between them in the RabbitMQ broker. The routing topology also controls how the transport uses the exchanges it creates to send and publish messages.

## Conventional Routing Topology

By default, the RabbitMQ transport uses the `ConventionalRoutingTopology`, which creates separate [fanout exchanges](https://www.rabbitmq.com/tutorials/amqp-concepts.html#exchange-fanout) for each message type, including inherited types, being published in the system. This means that polymorphic routing and multiple inheritance for events is supported since each subscriber will bind its input queue to the relevant exchanges based on the event types that it has handlers for.

WARNING: The RabbitMQ transport doesn't automatically modify or delete existing bindings. Because of this, when modifying the message class hierarchy, the existing bindings for the previous class hierarchy will still exist and should be deleted manually.


## Direct Routing Topology

The `DirectRoutingTopology` is another provided routing topology that routes all events through a single exchange, `amq.topic` by default. Events are published using a routing key based on the event type, and subscribers will use that key to filter their subscriptions.

To enable the direct routing topology, use the following configuration:

snippet:rabbitmq-config-usedirectroutingtopology

Adjust the conventions for exchange name and routing key by using the overload:

snippet:rabbitmq-config-usedirectroutingtopologywithcustomconventions


## Custom Routing Topology

If the routing topologies mentioned above aren't flexible enough, then take full control over how routing is done by implementing a custom routing topology. This is done by:

 1. Define the topology by creating a class implementing `IRoutingTopology`.
 1. Register it with the transport calling `UseRoutingTopology` as shown below.

snippet:rabbitmq-config-useroutingtopology