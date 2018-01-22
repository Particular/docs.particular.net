---
title: Transport Bridge
summary: How to connect parts of the system that use different transports 
component: Bridge
related:
- samples/azure/azure-service-bus-msmq-bridge
- samples/msmq/sql-bridge
reviewed: 2018-01-21
---

`NServiceBus.Bridge` is a component that allows to connect parts of NServiceBus-based solution that use different transports. Contrary to [Gateway](/nservicebus/gateway/) and [Wormhole](/nservicebus/wormhole), the Bridge handles both sends and publishes and it does so as transparently as possible:

 * The endpoint that *replies* to a message does not have to know if the initiating message came through a bridge. The reply will be automatically routed to the correct bridge and then forwarded to the initiator endpoint.
 * The endpoint that *publishes* events does not have to know if the subscribers are on the other side of the bridge.

Here's a comparison between the gateway technologies ([Gateway](/nservicebus/gateway/) and [Wormhole](/nservicebus/wormhole)) and the Bridge

|                       | Gateway/Wormhole | Bridge                        |
|-----------------------|------------------|-------------------------------|
| Supports sends        | Yes              | Yes                           |
| Supports replies      | Yes              | Yes                           |
| Replier aware         | Yes              | No                            |
| Supports publishes    | No               | Yes                           |
| Publisher aware       | N/A              | No                            |
| Dedicated routing     | Yes, via sites   | Yes, via *connector* configuration |
| Geo-distribution      | Yes              | No                            |
| Logically significant | Yes              | No                            |
|                       |                  |                               |

The main difference is that the Bridge connects parts of the system that have no logical distinction so the bridging is a purely technical exercise. In other words, if both sides could agree on the same transport technology, the Bridge could be removed without any changes to the logic of the system. Gateways on the other hand are logically significant and the idea of *sites* has to be taken into account in the logic itself.

The Bridge can handle both sends and publishes in a close-to-transparent manner, both for [unicast](/transports/#types-of-transports-unicast-only-transports) and [multicast](/transports/#types-of-transports-multicast-enabled-transports) transports.


## Topology

The Bridge is a process hosting a pair of NServiceBus endpoints that forward messages between each other. Regular endpoints connect to the Bridge using *connectors* that allow them to configure the routing

snippet: connector

The snippet above tells the endpoint that a designated Bridge listens on queue `LeftBank` and that messages of type `MyMessage` should be sent to the endpoint `Receiver` on the other side of the Bridge. It also tell the subscription infrastructure that the event `MyEvent` is published by the endpoint `Publisher` that is hosted on the other side of the bridge.


## Bridge configuration

The following snippet shows a simple MSMQ-to-RabbitMQ Bridge configuration

snippet: bridge

The Bridge has a simple life cycle:

snippet: lifecycle


## Subscribing

Subscribing to an event through a Bridge is always done via a message-driven mechanism similar to the one used by the [unicast transports](/transports/#types-of-transports-unicast-only-transports). The subscription message contains additional information about the name of the publisher endpoint and is send to the Bridge instead of the publisher.


## Publishing

Publishing an event that is subscribed by an endpoint on the other side of a Bridge does not differ from regular publishing. In fact the publisher does not need to know about the Bridge at all.


## Matching publishers and subscribers

The Bridge uses a [persistence-based message-driven](/nservicebus/messaging/publish-subscribe/#mechanics-persistence-based-message-driven) approach. Each Bridge has a subscription storage. When a subscribe message comes via a *connector*, a new entry in the subscription store is created matching the subscriber and the event type. Next, the subscribe request if forwarded to the other side of the Bridge where the execution depends on the type of the transport:


### Unicast transports

If the transport on the other side supports unicast operations only, the subscribe request is turned into a subscription message that is sent to the ultimate publisher. The subscriber address is the Bridge address so that the Bridge can forward the published events.


### Multicast transports

If the transport on the other side does support multicast operations, the subscriber request is turned into the native subscribe action. This might result in creation of topics, exchanges or similar structures in the underlying message broker.


## De-duplication

The messages traveling through a Bridge can get duplicated along the way. The Bridge does not come with an integrated message deduplication mechanism.

The Bridge does, however, preserve the message ID between the source and the ultimate destination. The message ID can be used to deduplicate at the destination. If the destination endpoint uses the [Outbox](/nservicebus/outbox/) the deduplication will be done automatically by means of the Outbox mechanism.
