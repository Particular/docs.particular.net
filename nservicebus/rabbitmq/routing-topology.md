---
title: Routing topology
reviewed: 2017-01-13
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

Every endpoint creates a queue with a name that is equal to the endpoint name. When an endpoint sends a message it sends it to a default exchange with a routing key equal to the destination endpoint name. This makes use of RabbitMQ [default exchanges](https://www.rabbitmq.com/tutorials/amqp-concepts.html) to move the message to a queue with the same name.


### Publishing using Direct Routing Topology

Every endpoint publishes an event using the `amq.topic` exchange with a routing key of the form 'Namespace.TypeName', corresponding to the type of the event. The event is moved to all queues that have a binding for that event type.

An endpoint that subscribes to a given event creates a binding to the default exchange with the appropriate routing key.

WARNING: In accordance with the [AMQP 0.9.1 standard](https://www.rabbitmq.com/amqp-0-9-1-reference.html#basic.publish.routing-key) the routing key has a length limit of 255 characters.


### Enabling Direct Routing Topology

To enable the direct routing topology, use the following configuration:

snippet:rabbitmq-config-usedirectroutingtopology

Adjust the conventions for exchange name and routing key by using the overload:

snippet:rabbitmq-config-usedirectroutingtopologywithcustomconventions

WARNING: In some cases, the direct routing topology may not deliver message types with "non-system" interfaces in their inheritance hierarchy. A "non-system" interface is any interface which is not contained in a .NET Framework assembly (any assembly signed with the same public key as mscorlib), and is not one of the [marker interfaces](/nservicebus/messaging/messages-events-commands.md#defining-messages-marker-interfaces). When using the direct routing topology, message types must not inherit from "non-system" interfaces. To guarantee delivery of message types which inherit from non-system interfaces, the conventional routing topology must be used.


## Custom Routing Topology

If the built-in routing topologies do not satisfy the requirements of the system, a custom routing topology may be used. To do this:

 1. Define the routing topology by creating a class implementing `IRoutingTopology`.
 1. Register it with the transport calling `UseRoutingTopology` as shown below.

partial: delegate-argument

snippet:rabbitmq-config-useroutingtopology

For each queue required by the endpoint, the transport will first declare that queue and will then call the `Initialize` method of the routing topology. The routing topology should then perform all initialization related to that specific queue such as the creation of appropriate exchanges and bindings.

partial: queue-declaration