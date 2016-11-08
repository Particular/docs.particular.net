---
title: Distributor and Publish-Subscribe
summary: How the Distributor behaves in a publish-subscribe scenario
reviewed: 2016-11-07
related:
 - nservicebus/messaging/publish-subscribe
redirects:
 - nservicebus/scalability-and-ha/publish-subscribe
---

Events can be received by multiple logical endpoints, however even in case of scale out each event will be received only by one physical instance of any logical subscriber.

The subscribe messages are sent by the workers but contain the distributor address in the `NServiceBus.ReplyTo` header. This causes all published events to be routed to the distributor instead of directly to the workers.

## Subscribe workflow

Compared to regular [subscribe workflow](/nservicebus/messaging/publish-subscribe.md#mechanics-persistence-based-message-driven-subscribe) the distributor variant contains an extra step -- forwarding the subscribe message from the distributor to the worker.

1. Subscribers request to a publisher the intent to subscribe to certain message types.
2. Distributor forwards the subscribe message to one of the workers
3. Worker stores the subscriber address and the message type in the persistence.

<!--

Participant Subscriber As Subscriber
Participant Publisher@Distributor As Disributor
Participant Publisher@Worker1 As Worker1
Participant Publisher@Worker1 As Worker2

Subscriber->Disributor: Subscribe to Message1
Disributor->Worker1: Subscribe to Message1
Worker1->Persistence: Store "Message1->Subscriber"

Subscriber->Disributor: Subscribe to Message2
Disributor->Worker2: Subscribe to Message2
Worker2->Persistence: Store "Message2->Subscriber"

-->

NOTE: It is very important that all workers share the same subscription persistence because subscribe messages are routed on a round-robin basis. This is different from [scaling out with Sender-Side Distribution](/nservicebus/msmq/sender-side-distribution.md) where each instance of subscriber gets a copy of subscribe message.


## Publish workflow

Compared to regular [publish workflow](/nservicebus/messaging/publish-subscribe.md#mechanics-persistence-based-message-driven-publish) the distributor variant contains an extra step -- forwarding the message from the distributor to the worker.

1. Some code (e.g. a saga or a handler) request that a message be published.
2. Publisher queries the storage for a list of subscribers.
3. Publisher loops through the list and sends a copy of that message to each subscriber. In this case the only subscriber is `Subscriber@Distributor` which is the address of the distributor node for the `Subscriber` endpoint.
4. Distributor takes the next worker from its ready queue and forwards the message to it. 

<!-- https://bramp.github.io/js-sequence-diagrams/

Participant Persistence As Persistence
Participant Publisher As Publisher
Participant Subscriber@Distributor As Distributor
Participant Subscriber@Worker1 As Worker1
Participant Subscriber@Worker2 As Worker2

Publisher->Publisher: endpoint.Publish(msg)
Publisher->+Persistence: Requests "who\nwants Message1"
Persistence-->-Publisher: "Subscriber@Distributor"
Publisher->Distributor: Send Message1
Distributor->Worker1: Send Message1

Publisher->Publisher: endpoint.Publish(msg)
Publisher->Distributor: Send Message1
Distributor->Worker2: Send Message1
-->

## Scaling out publishers

Publishers can also be scaled out using the distributor and it does not affect the workflow. Each worker node independently fetches the list of subscribers for each publish operation requested. 
