---
title: Messaging
summary: Outline the various approach to sending-receiving, defining messages and common messaging patterns.
reviewed: 2020-10-24
---

NServiceBus is an implementation of a [message based architecture](https://www.enterpriseintegrationpatterns.com/patterns/messaging/Messaging.html). As such it implements many of the common [messaging patterns](https://www.enterpriseintegrationpatterns.com/patterns/messaging/index.html)

For example:

 * [Messages, events and commands](/nservicebus/messaging/messages-events-commands.md) are an implementation of the [Message](https://www.enterpriseintegrationpatterns.com/patterns/messaging/Message.html), [Event](https://www.enterpriseintegrationpatterns.com/patterns/messaging/EventMessage.html) and [Command](https://www.enterpriseintegrationpatterns.com/patterns/messaging/CommandMessage.html) patterns.
 * [Persistence](/persistence/) is an implementation of the [Shared Database Pattern](https://www.enterpriseintegrationpatterns.com/patterns/messaging/SharedDataBaseIntegration.html)
 * [Publish Subscribe](/nservicebus/messaging/publish-subscribe/) is an implementation of the [Durable Subscription Pattern](https://www.enterpriseintegrationpatterns.com/patterns/messaging/DurableSubscription.html)


The approaches to messaging in NServiceBus can be categorized into the following:

 * [Messages, events and commands](/nservicebus/messaging/messages-events-commands.md)
 * [Contract Design](/nservicebus/messaging/messages-events-commands.md)
 * [Sending Messages](/nservicebus/messaging/send-a-message.md)
 * [Delayed delivery](/nservicebus/messaging/delayed-delivery.md) via the [timeout manager](/nservicebus/messaging/timeout-manager.md)
 * [Message Headers](/nservicebus/messaging/headers.md)
 * [Endpoint naming](/nservicebus/endpoints/specify-endpoint-name.md)
 * [Message Routing](/nservicebus/messaging/routing.md)
 * [Scheduling](/nservicebus/scheduling/)
 * [Outbox](/nservicebus/outbox/)
 * [Databus](/nservicebus/messaging/databus/)