---
title: Routing topology
reviewed: 2016-08-30
component: Rabbit
versions: '[2,]'
---


The RabbitMQ transport has the concept of a routing topology, which controls how it creates exchanges, queues, and the bindings between them in the RabbitMQ broker. The routing topology also controls how the transport uses the exchanges it creates to send and publish messages.


## Conventional Routing Topology

By default, the RabbitMQ transport uses the `ConventionalRoutingTopology`, which creates separate [fanout exchanges](https://www.rabbitmq.com/tutorials/amqp-concepts.html#exchange-fanout) for each message type being published in the system and for each endpoint.


### Sending using Conventional Routing Topology

Every endpoint creates a queue and an exchange with names equal to the endpoint name and a binding is set up to move all messages to that queue. When an endpoint sends a message it sends it to an exchange with a name matching the destination endpoint name and the message is then moved to the queue of the same name.


### Publishing using Conventional Routing Topology

Every endpoint, before publishing an event, creates a fanout exchange for the event and it's base types. Every exchange has a name of the form `Namespace.TypeName`, corresponding to the type of the event. Bindings are created to ensure that base type events also are moved to the exchanges for the derived types when they are published. An endpoint that subscribes to a given event type looks for the exchange with the appropriate name and creates a binding to move messages of that type to its queue.

This means that polymorphic routing and multiple inheritance for events is supported since each subscriber will bind its input queue to the relevant exchanges based on the event types that it has handlers for.


## Direct Routing Topology

The `DirectRoutingTopology` routes all events through a single exchange, `amq.topic` by default. Events are published using a routing key based on the event type, and subscribers will use that key to filter their subscriptions.


### Sending using Direct Routing Topology

Every endpoint creates a queue with name that is equal to the endpoint name. When an endpoint sends a message it sends it to a default exchange with a routing key equal to the destination endpoint name. This makes use of RabbitMQ [default exchanges](https://www.rabbitmq.com/tutorials/amqp-concepts.html) to move the message to a queue with the same name.


### Publishing using Direct Routing Topology

Every endpoint publishes an event using the `amq.topic` exchange with a routing key of the form 'Namespace.TypeName', corresponding to the type of the event. The event is moved to all queues that have a binding for that event type.

An endpoint that subscribes to a given event creates a binding to the default exchange with the appropriate routing key.

WARNING: Routing key has a length limitation of 255 chars coming from AMQP 0.9.1 standard. For more details see [RabbitMQ documentation](http://www.rabbitmq.com/amqp-0-9-1-reference.html#basic.publish.routing-key).

### Enabling Direct Routing Topology

To enable the direct routing topology, use the following configuration:

snippet:rabbitmq-config-usedirectroutingtopology

Adjust the conventions for exchange name and routing key by using the overload:

snippet:rabbitmq-config-usedirectroutingtopologywithcustomconventions


## Custom Routing Topology

If the above routing topologies aren't flexible enough, it is possible to take full control over routing by implementing a custom routing topology. To do this:

 1. Define the topology by creating a class implementing `IRoutingTopology`.
 1. Register it with the transport calling `UseRoutingTopology` as shown below.

snippet:rabbitmq-config-useroutingtopology
