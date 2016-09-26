---
title: Routing topology
reviewed: 2016-08-30
component: Rabbit
versions: '[2,]'
---


The RabbitMQ transport has the concept of a routing topology, which controls how it creates exchanges, queues, and the bindings between them in the RabbitMQ broker. The routing topology also controls how the transport uses the exchanges it creates to send and publish messages.

## Conventional Routing Topology

By default, the RabbitMQ transport uses the `ConventionalRoutingTopology`, which creates separate [fanout exchanges](https://www.rabbitmq.com/tutorials/amqp-concepts.html#exchange-fanout) for each message type being published in the system and for each endpoint. This means that polymorphic routing and multiple inheritance for events is supported since each subscriber will bind its input queue to the relevant exchanges based on the event types that it has handlers for. 

### Sending using Conventional Routing Topology

Every endpoint creates a queue and an exchange which names are equal to endpoint name and a binding is set up to move all messages to that queue. When endpoint sends message it sends it to an exchange which name matches destination name, which then is move to the queue of the same name. 

### Publishing using Conventional Routing Topology

Every endpoint before publishing an event creates a fanout exchange for the event and it's base types. Every exchange has a name matching `namespace.eventType` of that event. Bindings are established to ensure that publishing base type events also are moved to sub-type exchanges. Endpoint that subscribes to given event looks for exchange of given name and set up binding to move those messages to its queue. 

## Direct Routing Topology

The `DirectRoutingTopology` is another provided routing topology that routes all events through a single exchange, `amq.topic` by default. Events are published using a routing key based on the event type, and subscribers will use that key to filter their subscriptions.

### Sending using Direct Routing Topology

Every endpoint creates a queue with name that is equal to endpoint name. When endpoint sends message it sends it to a default exchange using as a routing key equals to destination name. That uses rabbitmq functionality of [default exchanges](https://www.rabbitmq.com/tutorials/amqp-concepts.html) to pass the message to a queue with the same name.

### Publishing using Direct Routing Topology

Endpoints publish an event using default exchange with a routing key that match events 'namespace.eventType', that event is passed to all queues that has binding for that event type.

Endpoint that subscribes to given event set-up binding to a default exchange with a routing key matching eventType. 

### Enabling Direct Routing Topology

To enable the direct routing topology, use the following configuration:

snippet:rabbitmq-config-usedirectroutingtopology

Adjust the conventions for exchange name and routing key by using the overload:

snippet:rabbitmq-config-usedirectroutingtopologywithcustomconventions

## Custom Routing Topology

If the routing topologies mentioned above aren't flexible enough, then take full control over how routing is done by implementing a custom routing topology. This is done by:

 1. Define the topology by creating a class implementing `IRoutingTopology`.
 1. Register it with the transport calling `UseRoutingTopology` as shown below.

snippet:rabbitmq-config-useroutingtopology