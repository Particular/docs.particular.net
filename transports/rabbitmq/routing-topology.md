---
title: Routing topology
reviewed: 2017-01-13
component: Rabbit
versions: '[2,]'
related:
 - nservicebus/operations
redirects:
 - nservicebus/rabbitmq/routing-topology
---


The RabbitMQ transport has the concept of a routing topology, which controls how it creates exchanges, queues, and the bindings between them in the RabbitMQ broker. The routing topology also controls how the transport uses the rabbitmq-config-useroutingtopologyexchanges it creates to send and publish messages.


## Conventional Routing Topology

By default, the RabbitMQ transport uses the `ConventionalRoutingTopology`, which relies on [fanout exchanges](https://www.rabbitmq.com/tutorials/amqp-concepts.html#exchange-fanout) to route messages. 


### Sending using Conventional Routing Topology

Each endpoint creates its own fanout exchange and queue, using its own name as the name of the exchange and queue. It also creates a binding between the exchange and queue. Messages are sent to the endpoint by sending them to the endpoint's exchange. The binding then routes the message to the endpoint's queue.


### Publishing using Conventional Routing Topology

For each type being published, a series of fanout exchanges are created to model the inheritance hierarchy of the type. For each type involved, an exchange is created, named in the following format: `Namespace:TypeName`. Bindings are created between the types, going from child to parent, until the entire hierarchy has been modeled. Exchanges are also created for each interface the type implements.

When an endpoint subscribes to an event, it first ensures that the above infrastructure exists. It then adds a binding from the exchange corresponding to the subscribed type to its own exchange.

When an endpoint publishes an event, it first ensures that the above infrastructure exists. It then sends the message to the exchange corresponding to the type being published.


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

snippet: rabbitmq-config-usedirectroutingtopology

Adjust the conventions for exchange name and routing key by using the overload:

snippet: rabbitmq-config-usedirectroutingtopologywithcustomconventions

WARNING: In some cases, the direct routing topology may not deliver message types with "non-system" interfaces in their inheritance hierarchy. A "non-system" interface is any interface which is not contained in a .NET Framework assembly (any assembly signed with the same public key as mscorlib), and is not one of the [marker interfaces](/nservicebus/messaging/messages-events-commands.md#defining-messages-marker-interfaces). When using the direct routing topology, message types must not inherit from "non-system" interfaces. To guarantee delivery of message types which inherit from non-system interfaces, the conventional routing topology must be used.


## Custom Routing Topology

If the built-in routing topologies do not satisfy the requirements of the system, a custom routing topology may be used. To do this:

 1. Define the routing topology by creating a class implementing `IRoutingTopology`.
 1. Register it with the transport calling `UseRoutingTopology` as shown below.

partial: delegate-argument

partial: argument

For each queue required by the endpoint, the transport will first declare that queue and will then call the `Initialize` method of the routing topology. The routing topology should then perform all initialization related to that specific queue such as the creation of appropriate exchanges and bindings.

partial: queue-declaration

partial: support-delayed-delivery
