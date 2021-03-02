---
title: Distributor and Publish-Subscribe
summary: Distributor behavior in a publish-subscribe scenario
reviewed: 2021-03-02
related:
 - nservicebus/messaging/publish-subscribe
redirects:
 - nservicebus/scalability-and-ha/publish-subscribe
 - nservicebus/msmq/distributor/publish-subscribe
---

Events can be received by multiple logical endpoints, however even in case of scale out each event will be received only by one physical instance of any logical subscriber.

The subscribe messages are sent by the workers but contain the distributor address in the [`NServiceBus.ReplyToAddress`](/nservicebus/messaging/headers.md#messaging-interaction-headers-nservicebus-replytoaddress) header. This causes all published events to be routed to the distributor instead of directly to the workers.


## Subscribe workflow

Compared to regular [subscribe workflow](/nservicebus/messaging/publish-subscribe/#mechanics-message-driven-persistence-based-subscribe) the distributor variant contains an extra step -- forwarding the subscribe message from the distributor to the worker.

 1. Subscribers request to a publisher the intent to subscribe to certain message types.
 1. Distributor forwards the subscribe message to one of the workers
 1. Worker stores the subscriber address and the message type in the persistence.

```mermaid
sequenceDiagram

Participant Subscriber
Participant Publisher@Distributor
Participant Publisher@Worker1
Participant Publisher@Worker1
Participant Persistence

Subscriber->>Publisher@Distributor: Subscribe to Message1
Publisher@Distributor->>Publisher@Worker1: Subscribe to Message1
Publisher@Worker1->>Persistence: Store "Message1->Subscriber"

Subscriber->>Publisher@Distributor: Subscribe to Message2
Publisher@Distributor->>Publisher@Worker2: Subscribe to Message2
Publisher@Worker2->>Persistence: Store "Message2->Subscriber"
```

NOTE: It is very important that all workers share the same subscription persistence because subscribe messages are routed on a round-robin basis. This is different from [scaling out with Sender-Side Distribution](/transports/msmq/sender-side-distribution.md) where each instance of subscriber gets a copy of subscribe message.


## Publish workflow

Compared to regular [publish workflow](/nservicebus/messaging/publish-subscribe/#mechanics-message-driven-persistence-based-publish) the distributor variant contains an extra step -- forwarding the message from the distributor to the worker.

 1. Some code (e.g. a saga or a handler) request a message to be published.
 1. Publisher queries the storage for a list of subscribers.
 1. Publisher loops through the list and sends a copy of that message to each subscriber. In this case, the only subscriber is `Subscriber@Distributor` which is the address of the distributor node for the `Subscriber` endpoint.
 1. Distributor takes the next worker from its ready queue and forwards the message to it. 

```mermaid
sequenceDiagram

Participant Persistence
Participant Publisher
Participant Subscriber@Distributor
Participant Subscriber@Worker1
Participant Subscriber@Worker2

Note over Publisher: Publish Message1 occurs
Publisher->>Persistence: Requests "who\nwants Message1"
Persistence->>Publisher: "Subscriber@Distributor"
Publisher->>Subscriber@Distributor: Send Message1
Subscriber@Distributor->>Subscriber@Worker1: Send Message1

Note over Publisher: Publish Message1 occurs
Publisher->>Persistence: Requests "who\nwants Message1"
Persistence->>Publisher: "Subscriber@Distributor"
Publisher->>Subscriber@Distributor: Send Message1
Subscriber@Distributor->>Subscriber@Worker2: Send Message1
```

## Scaling out publishers

Publishers can also be scaled out using the distributor and it does not affect the workflow. Each worker node independently fetches the list of subscribers for each publish operation requested. 
