---
title: Publish-Subscribe
summary: Subscribers tell the publisher they are interested. Publishers store addresses for sending messages.
reviewed: 2025-10-14
component: Core
redirects:
 - nservicebus/how-pub-sub-works
 - nservicebus/messaging/publish-subscribe/how-to-pub-sub
 - nservicebus/how-to-pub-sub-with-nservicebus
 - nservicebus/publish-subscribe-configuration
 - nservicebus/messaging/publish-subscribe/configuration
related:
 - samples/pubsub
 - tutorials/nservicebus-step-by-step
 - nservicebus/messaging/messages-events-commands
 - nservicebus/messaging/headers
 - transports/msmq/subscription-authorisation
---

NServiceBus has a built-in implementation of the [Publish-subscribe pattern](https://en.wikipedia.org/wiki/Publish%E2%80%93subscribe_pattern).

> Publish-subscribe is a messaging pattern where senders of messages, called publishers, do not program the messages to be sent directly to specific receivers, called subscribers. Instead, published messages are characterized into classes, without knowledge of what, if any, subscribers there may be. Similarly, subscribers express interest in one or more classes, and only receive messages that are of interest, without knowledge of what, if any, publishers there are.

Or, in simpler terms

> Subscribers let the publisher know they're interested, and the publisher stores their addresses to know where to send which message.

## Mechanics

Depending on the features provided by a given transport, there are two possible implementations of Publish-Subscribe mechanics: message-driven (persistence-based) and native.

> [!NOTE]
> For simplicity, these explanations refer to specific endpoints as "Subscribers" and "Publishers." However, in reality, any endpoint can be both a publisher and/or a subscriber.

### All subscribers get their copy of the event

To ensure that each subscriber can process and potentially retry the event independently of other subscribers, NServiceBus ensures that each subscriber receives a copy of the published event delivered to their input queue.

### Native

For multicast transports that [support publish–subscribe natively](/transports/types.md#multicast-enabled-transports) neither persistence nor control message exchange is required to complete the publish-subscribe workflow.

#### Subscribe

The subscribe workflow for multicast transports is as follows:

 1. Subscribers send a request to the broker with the intent to subscribe to specific message types.
 1. The broker stores the subscription information.

Note that the publisher does not interact with the subscribe workflow in this case.

```mermaid
sequenceDiagram

Participant Subscriber1 As Subscriber1
Participant Subscriber2 As Subscriber2
Participant Broker As Broker
Participant Publisher As Publisher
Subscriber1 ->> Broker: Subscribe to Message1
Subscriber2 ->> Broker: Subscribe to Message1
```

#### Publish

The publish workflow for multicast transports is as follows:

 1. Some code (e.g., a saga or a handler) requests that a message be published.
 1. The publisher sends the message to the Broker.
 1. The broker sends a **copy of that message** to each subscriber.

```mermaid
sequenceDiagram

Participant Subscriber1 As Subscriber1
Participant Subscriber2 As Subscriber2
Participant Transport As Transport
Note over Publisher: Publish Message1 occurs
Publisher ->> Transport: Sends Message1
Transport ->> Subscriber1: Send Message1
Transport ->> Subscriber2: Send Message1
```

### Message-driven (persistence-based)

Message-driven publish-subscribe is controlled by *subscribe* and *unsubscribe* system messages the subscriber sends to the publisher. The message-driven publish-subscribe implementation is used by the [unicast transports](/transports/types.md#unicast-only-transports). These transports are limited to unicast (point-to-point) communication and have to simulate multicast delivery via a series of point-to-point communications.

#### Subscribe

The subscribe workflow for unicast transports is as follows:

 1. Subscribers request to a publisher the intent to subscribe to specific message types.
 1. Publisher stores the subscriber names and the message types in the persistence.

```mermaid
sequenceDiagram

Participant Subscriber1 As Subscriber1
Participant Subscriber2 As Subscriber2
Subscriber1 ->> Publisher: Subscribe to Message1
Publisher ->> Persistence: Store "Subscriber1 wants Message1"
Subscriber2 ->> Publisher: Subscribe to Message1
Publisher ->> Persistence: Store "Subscriber2 wants Message1"
```

The publisher's address is provided via [routing configuration](/nservicebus/messaging/routing.md).

#### Publish

Message-driven publish-subscribe relies on the publisher's access to a persistent store to maintain the mapping between message types and their subscribers.

Available subscription persisters include

 * [MSMQ](/persistence/msmq)
 * [RavenDB](/persistence/ravendb)
 * [NHibernate](/persistence/nhibernate)
 * [Non-Durable](/persistence/non-durable)
 * [Azure Storage](/persistence/azure-table)
 * [SQL Server](/persistence/sql)

The publish workflow for [unicast transports](/transports/types.md#unicast-only-transports) is as follows:

 1. Some code (e.g., a saga or a handler) requests that a message be published.
 1. The publisher queries the storage for a list of subscribers.
 1. The publisher loops through the list and sends each subscriber a **copy of that message**.

```mermaid
sequenceDiagram

Participant Subscriber1 As Subscriber1
Participant Subscriber2 As Subscriber2
Note over Publisher: Publish Message1 occurs
Publisher ->> Persistence: Requests "who wants Message1"
Persistence ->> Publisher: Subscriber1 and Subscriber2
Publisher ->> Subscriber1: Send Message1
Publisher ->> Subscriber2: Send Message1
```

partial: disable-publishing
