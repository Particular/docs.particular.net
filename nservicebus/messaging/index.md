---
title: Messaging
summary: Outline the various approach to sending-receiving, defining messages and common messaging patterns.
reviewed: 2025-10-03
---

NServiceBus is an implementation of a [message-based architecture](https://www.enterpriseintegrationpatterns.com/patterns/messaging/Messaging.html). As such it implements many of the common [messaging patterns](https://www.enterpriseintegrationpatterns.com/patterns/messaging/index.html) to support distributed, decoupled systems

##  Architectural Foundations

NServiceBus applies key messaging patterns to solve common challenges in distributed systems:

* [Messages, events and commands](/nservicebus/messaging/messages-events-commands.md) are an implementation of the [Message](https://www.enterpriseintegrationpatterns.com/patterns/messaging/Message.html), [Event](https://www.enterpriseintegrationpatterns.com/patterns/messaging/EventMessage.html) and [Command](https://www.enterpriseintegrationpatterns.com/patterns/messaging/CommandMessage.html) patterns.
* [Persistence](/persistence/) is an implementation of the [Shared Database Pattern](https://www.enterpriseintegrationpatterns.com/patterns/messaging/SharedDataBaseIntegration.html)
* [Publish-Subscribe](/nservicebus/messaging/publish-subscribe/) is an implementation of the [Durable Subscription Pattern](https://www.enterpriseintegrationpatterns.com/patterns/messaging/DurableSubscription.html)

## Messaging Concepts in NServiceBus

The approaches to messaging in NServiceBus can be categorized into the following concepts:

* [Messages, events and commands](/nservicebus/messaging/messages-events-commands.md)
* [Contract design](/nservicebus/messaging/messages-events-commands.md)
* [Sending messages](/nservicebus/messaging/send-a-message.md)
* [Delayed delivery](/nservicebus/messaging/delayed-delivery.md)
* [Message headers](/nservicebus/messaging/headers.md)
* [Endpoint naming](/nservicebus/endpoints/specify-endpoint-name.md)
* [Message routing](/nservicebus/messaging/routing.md)
* [Scheduling](/nservicebus/scheduling/)
* [Outbox](/nservicebus/outbox/)
* [Databus](/nservicebus/messaging/claimcheck/)
