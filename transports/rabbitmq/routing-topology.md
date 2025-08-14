---
title: Routing topology
summary: Learn about the routing topology options in RabbitMQ and how they impact NServiceBus systems
reviewed: 2025-08-14
component: Rabbit
versions: '[2,]'
related:
 - nservicebus/operations
redirects:
 - nservicebus/rabbitmq/routing-topology
---

The RabbitMQ transport uses a *routing topology* to control how exchanges, queues, and bindings are created in the broker. The routing topology also defines how these exchanges are used for sending and publishing messages.  

All endpoints in a system must use the same routing topology to communicate. For new systems, use the [conventional routing topology](routing-topology.md#conventional-routing-topology). The [direct routing topology](routing-topology.md#direct-routing-topology) should only be used when adding endpoints to an existing system that already relies on it. A custom routing topology may be useful when integrating with legacy systems.

---

## Conventional routing topology

The conventional routing topology uses [fanout exchanges](https://www.rabbitmq.com/tutorials/amqp-concepts.html#exchange-fanout) to route messages.

> [!NOTE]  
> This is the recommended routing topology. It was the default prior to NServiceBus.RabbitMQ version 5.

### Sending

Each endpoint creates a *fanout* exchange and a queue with the same name as the endpoint. A binding connects them.  
Messages are sent by publishing them to the endpoint’s exchange, which then routes them to the queue.

### Publishing

For each event type, a set of *fanout* exchanges is created to represent its inheritance hierarchy.  
- Each type has an exchange named `Namespace:TypeName`.  
- Bindings connect child types to their parents, modeling the full hierarchy.  
- Exchanges are also created for each implemented interface.  

When subscribing, an endpoint ensures the infrastructure exists and then adds a binding from the subscribed type’s exchange to its own exchange.  

When publishing, the endpoint ensures the infrastructure exists and then publishes to the exchange of the event type.

### Enabling

snippet: rabbitmq-config-useconventionalroutingtopology

## Direct routing topology

The direct routing topology routes all events through a single exchange (default: `amq.topic`). Events are published with a routing key based on the event type, and subscribers use that key to filter messages.

### Sending

Each endpoint creates a queue named after the endpoint.

When sending, the message is published to the [default exchange](https://www.rabbitmq.com/tutorials/amqp-concepts.html) with a routing key equal to the destination endpoint name. This leverages RabbitMQ default exchanges to deliver messages directly to the queue.

### Publishing

Every endpoint publishes an event using the `amq.topic` exchange with a *routing key* of the form `Namespace.TypeName`, corresponding to the type of the event. The event is moved to all queues that have a binding for that event type.

An endpoint that subscribes to a given event creates a binding to the default exchange with the appropriate *routing key*.

> [!WARNING]
> In accordance with the [AMQP 0.9.1 standard](https://www.rabbitmq.com/amqp-0-9-1-reference.html#basic.publish.routing-key) the *routing key* has a length limit of 255 characters.

### Enabling

To enable the direct routing topology, use the following configuration:

snippet: rabbitmq-config-usedirectroutingtopology

### Overriding conventions

The default conventions for exchange names and routing keys can be overridden by using the following overload:

snippet: rabbitmq-config-usedirectroutingtopologywithcustomconventions

> [!WARNING]
> In some cases, the direct routing topology may not deliver message types with "non-system" interfaces in their inheritance hierarchy. A "non-system" interface is any interface which is not contained in a .NET Framework assembly (any assembly signed with the same public key as mscorlib), and is not one of the [interfaces](/nservicebus/messaging/messages-events-commands.md#identifying-messages). When using the direct routing topology, message types must not inherit from "non-system" interfaces. To guarantee delivery of message types which inherit from non-system interfaces, the conventional routing topology must be used.

partial: queue-type

## Controlling exchange and queue durability

The routing topologies provided by the transport create durable exchanges and queues by default. To create transient exchanges and queues, use the following:

snippet: rabbitmq-disable-durable-exchanges


## Custom routing topology

If the built-in routing topologies do not satisfy the requirements of the system, a custom routing topology may be used. To do this:

 1. Define the routing topology by creating a class implementing `IRoutingTopology`.
 1. Register it with the transport as shown below:

snippet: rabbitmq-config-useroutingtopologyDelegate

The boolean argument supplied to the factory delegate indicates whether the custom routing topology should create durable exchanges and queues on the broker. Read more about durable exchanges and queues in the [AMQP Concepts Guide](https://www.rabbitmq.com/tutorials/amqp-concepts.html).
