---
title: Routing topology
summary: Information about the routing topology options available in RabbitMQ and how they impact NServiceBus systems
reviewed: 2020-08-12
component: Rabbit
versions: '[2,]'
related:
 - nservicebus/operations
redirects:
 - nservicebus/rabbitmq/routing-topology
---


The RabbitMQ transport has the concept of a routing topology, which controls how it creates exchanges, queues, and the bindings between them in the RabbitMQ broker. The routing topology also controls how the transport uses the exchanges it creates to send and publish messages. All endpoints in a system must use the same topology to be able to communicate with each other. For new systems, the [conventional routing topology](routing-topology.md#conventional-routing-topology) should be used. The [direct routing topology](routing-topology.md#direct-routing-topology) is recommended only when adding an endpoint to an existing system that already uses that topology. A custom topology can be useful when integrating with a legacy system.


## Conventional routing topology

The conventional routing topology relies on [fanout exchanges](https://www.rabbitmq.com/tutorials/amqp-concepts.html#exchange-fanout) to route messages. 

partial: mandatory


### Sending using the conventional routing topology

Each endpoint creates a pair of a *fanout* exchange and a queue named after the endpoint's name. It also creates a binding between them. Messages are sent to the endpoint by publishing them to the endpoint's exchange. The binding then routes the message to the endpoint's queue.


### Publishing using the conventional routing topology

For each type being published, a series of *fanout* exchanges are created to model the inheritance hierarchy of the type. For each type involved, an exchange is created, named in the following format: `Namespace:TypeName`. Bindings are created between the types, going from child to parent, until the entire hierarchy has been modeled. Exchanges are also created for each interface the type implements.

When an endpoint subscribes to an event, it first ensures that the above infrastructure exists. It then adds a binding from the exchange corresponding to the subscribed type to its own exchange.

When an endpoint publishes an event, it first ensures that the above infrastructure exists. It then publishes the message to the exchange corresponding to the message type being published.


partial: enable-conventional-routing-topology


## Direct routing topology

The direct routing topology routes all events through a single exchange, `amq.topic` by default. Events are published using a *routing key* based on the event type, and subscribers will use that key to filter their subscriptions.


### Sending using the direct routing topology

Every endpoint creates a queue named after the endpoint's name. When an endpoint sends a message it publishes it to a default exchange with a *routing key* equal to the destination endpoint name. This makes use of RabbitMQ [default exchanges](https://www.rabbitmq.com/tutorials/amqp-concepts.html) to move the message to a queue with the same name.


### Publishing using the direct routing topology

Every endpoint publishes an event using the `amq.topic` exchange with a *routing key* of the form `Namespace.TypeName`, corresponding to the type of the event. The event is moved to all queues that have a binding for that event type.

An endpoint that subscribes to a given event creates a binding to the default exchange with the appropriate *routing key*.

WARNING: In accordance with the [AMQP 0.9.1 standard](https://www.rabbitmq.com/amqp-0-9-1-reference.html#basic.publish.routing-key) the *routing key* has a length limit of 255 characters.


### Enabling the direct routing topology

To enable the direct routing topology, use the following configuration:

snippet: rabbitmq-config-usedirectroutingtopology

### Overriding the default conventions

The default conventions for exchange names and routing keys can be overridden by using the following overload:

snippet: rabbitmq-config-usedirectroutingtopologywithcustomconventions

WARNING: In some cases, the direct routing topology may not deliver message types with "non-system" interfaces in their inheritance hierarchy. A "non-system" interface is any interface which is not contained in a .NET Framework assembly (any assembly signed with the same public key as mscorlib), and is not one of the [interfaces](/nservicebus/messaging/messages-events-commands.md#identifying-messages). When using the direct routing topology, message types must not inherit from "non-system" interfaces. To guarantee delivery of message types which inherit from non-system interfaces, the conventional routing topology must be used.


partial: exchange-queue-durability


## Custom routing topology

If the built-in routing topologies do not satisfy the requirements of the system, a custom routing topology may be used. To do this:

 1. Define the routing topology by creating a class implementing `IRoutingTopology`.
 1. Register it with the transport calling `UseRoutingTopology` as shown below.

partial: custom-delegate-argument

partial: custom-no-argument

partial: transport-queue-declaration

partial: control-queue-declaration

partial: support-delayed-delivery
