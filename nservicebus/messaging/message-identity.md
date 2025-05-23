---
title: Message Identity
reviewed: 2025-01-05
component: Core
---


Each message dispatched from an [Endpoint](/nservicebus/endpoints/) has a unique identity. NServiceBus generates and stores this value in the [`NServiceBus.MessageId` header](/nservicebus/messaging/headers.md#messaging-interaction-headers-nservicebus-messageid).

Many features take advantage of message identity. For example, the [Outbox](/nservicebus/outbox) uses message identity to deduplicate messages and [recoverability](/nservicebus/recoverability/) uses message identity to keep track of how many times the endpoint has tried to process a message.

> [!NOTE]
> Message identities are not guaranteed to be unique across endpoints from a processing perspective. For example, the message identity of a published event will always be the same for all subscribers. In cases where a globally unique identity for processed messages is required, the name of the processing [logical endpoint](/nservicebus/endpoints/#logical-endpoints) should be combined with the message identity.

## Specifying message identity

Message identity can be explicitly specified, overriding the default identity provided by NServiceBus.

> [!WARNING]
> It is important that the strategy used to generate message identities results in globally unique identifiers. If two messages ever have the same identity then some features will treat them as the same message. This will cause errors which are difficult to diagnose.

Specify message identity using the `SendOptions` class.

snippet: MessageId-SendOptions
