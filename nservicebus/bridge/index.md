---
title: Transport Bridge
summary: How to connect parts of the system that use different transports 
component: Bridge
related:
- samples/azure/azure-service-bus-msmq-bridge
- samples/msmq/sql-bridge
reviewed: 2018-05-04
---

`NServiceBus.Bridge` is a component that connects parts of an NServiceBus-based solution that use different transports. Unlike the [gateway](/nservicebus/gateway/), NServiceBus.Bridge handles both sending and publishing. The main difference is that the bridge connects parts of the system that have no logical distinction so bridging is a purely technical exercise. In other words, if both sides agree on the same transport technology, the bridge could be removed without any changes to the logic of the system. Gateways on the other hand are logically significant and the idea of *sites* must be taken into account in the logic itself.

Here's a comparison between the [gateway](/nservicebus/gateway/) and the bridge

|                       | Gateway          | Bridge                        |
|-----------------------|------------------|-------------------------------|
| Supports sends        | Yes              | Yes                           |
| Supports replies      | Yes              | Yes                           |
| Replier aware         | Yes              | No                            |
| Supports publishes    | No               | Yes                           |
| Publisher aware       | N/A              | No                            |
| Dedicated routing     | Yes, via sites   | Yes, via *connector* configuration |
| Geo-distribution      | Yes              | Yes, via inter-bridge forwarding |
| Logically significant | Yes              | No                            |
|                       |                  |                               |

As stated above, the main difference is that the bridge can handle publishing. The other significant difference is that the bridge is transparent to the publishing and replying endpoint. That is:

 * The endpoint that *replies* to a message does not have to know if the initiating message came through a bridge. The reply will be routed automatically to the correct bridge and then forwarded to the initiating endpoint.
 * The endpoint that *publishes* events does not have to know if the subscribers are on the other side of the bridge.

Both bridges and gateways require a dedicated routing configuration that replaces the standard [NServiceBus routing](/nservicebus/messaging/routing.md).

Both technologies can be applied to build a geographically distributed system. By virtue of being a more general-purpose solution, the bridge can emulate the gateway routing behavior in a geo-distributed deployment.


## Routing configuration

Regular endpoints connect to the bridge using *connectors* that allow them to configure the routing

snippet: connector

The snippet above tells the endpoint that a designated bridge listens on queue `LeftBank` and that messages of type `MyMessage` should be sent to the endpoint `Receiver` on the other side of the bridge. It also tell the subscription infrastructure that the event `MyEvent` is published by the endpoint `Publisher` that is hosted on the other side of the bridge.


## Bridge configuration

The following snippet shows a simple MSMQ-to-RabbitMQ bridge configuration

snippet: bridge

The bridge has a simple life cycle:

snippet: lifecycle


## Topologies

A single bridge is a pair of NServiceBus endpoints that forward messages between each other. Each side of the bridge uses a different transport and consists of a message pump attached to a queue and a dispatcher that can send messages to different queues via the same transport mechanism.


### Simple

![Simple bridge](simple.svg)

The arrows show the path of messages sent from `Endpoint A` to `Endpoint C` and from `Endpoint D` to `Endpoint B`. The messages cross the bridge in the opposite direction. Each message is initially sent to the bridge queue (based on the routing configuration of the connector) and then forwarded through a dispatcher on the other side of the bridge. There is one additional *hop* compared to a direct communication of endpoints.


### Switch

![Switch](switch.svg)

A _switch_ is a generalization of a bridge. It consists of `n` NServiceBus endpoints connected via an in-memory backplane for routing messages. A single switch can connect more than two transports. These transports don't necessarily need to be different technologies. In fact it is uncommon to use more than two different transport technologies in a single solution. A more common scenario is using multiple instances of a single transport technology e.g. Azure Service Bus namespaces or RabbitMQ clusters. The switch can be used to route messages between them.

### Backplane

![Backplane](backplane.svg)

Two or more bridges can be connected together to form a _backplane_ topology. In the example above the Green transport is used as a backplane for passing messages between the Blue and Red parts of the system. This topology is useful when the system consists of multiple geographically distributed parts. The local transports (Blue and Red) can be optimized for throughput and/or consistency while the backplane transport can be optimized for geo-distribution capabilities e.g. MSMQ can be used locally while Azure Storage Queues can be used to connect the sites.

In this topology the routing can be configured in two ways.

#### Bridge-style

In this mode the routing is similar to the simple case with one bridge. The endpoints need to be configured to route messages through their local bridge. The sites are not logically significant and each endpoint is deployed to exactly one site.

#### Gateway-style

In this mode the routing is similar to the [gateway](/nservicebus/gateway/) routing i.e. when sending a message the sender specifies the site(s) to route messages to. Multiple sites can contain the same logical endpoint or even be exact copies. This type of topology would be useful for a warehouse company. The software system for that company could have one site for the headquarters and multiple identical warehouse sites. When an endpoint in headquarters sends a message to an endpoint in one of the warehouses it needs to specify the ID of the destination site explicitly.
  

## Publish/Subscribe

The following section describes the behavior of the bridge with regards to publishing and subscribing to events.

### Subscribing

Subscribing to an event through a bridge is always done via a message-driven mechanism similar to the one used by the [unicast transports](/transports/types.md#unicast-only-transports). The subscription message contains additional information about the name of the publisher endpoint and is send to the bridge instead of the publisher.


### Publishing

Publishing an event that is subscribed to by an endpoint on the other side of a bridge does not differ from regular publishing. In fact the publisher does not need to know about the bridge at all.


### Matching publishers and subscribers

The bridge uses a [message-driven](/nservicebus/messaging/publish-subscribe/#mechanics-message-driven-persistence-based) approach. Each bridge has a subscription storage. When a subscribe message comes via a *connector*, a new entry in the subscription store is created matching the subscriber and the event type. Next, the subscribe request if forwarded to the other side of the bridge where the execution depends on the type of the transport:


#### Unicast transports

If the transport on the other side supports unicast operations only, the subscribe request is turned into a subscription message that is sent to the ultimate publisher. The subscriber address is the bridge address so that the bridge can forward the published events.


#### Multicast transports

If the transport on the other side does support multicast operations, the subscriber request is turned into the native subscribe action. This might result in creation of topics, exchanges or similar structures in the underlying message broker.


## Deduplication

The messages travelling through a bridge can get duplicated along the way. The bridge does not come with an integrated message de-duplication mechanism but offers an extension point in the the form of *interceptors* so a custom deduplication algorithm can be plugged in. The [backplane sample](/samples/bridge/backplane) demonstrates this feature.

However, the bridge does preserve the message ID between the source and the ultimate destination. The message ID can be used to de-duplicate at the destination. If the destination endpoint uses the [outbox](/nservicebus/outbox/) the de-duplication will be done automatically by means of the outbox mechanism.
