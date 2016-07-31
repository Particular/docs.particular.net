---
title: Message Identity
reviewed: 2016-07-21
summary: How are individual messages identified?
---


Each message that is dispatched from an [Endpoint](/nservicebus/endpoints/) has a unique identity. NServiceBus generates and stores this value as a [header](/nservicebus/messaging/headers.md) on the message named `NServiceBus.MessageId`. If the selected transport supports message identity, and there is value in using the NServiceBus MessageId, this value can be optionally used as the transport level message identity as well.

Many features take advantage of message identity. For example, the [Outbox](/nservicebus/outbox) uses message identity to deduplicate messages and [recoverability](/nservicebus/recoverability/) uses message identity to keep track of how many times the endpoint has tried to process a message.


## Specifying message identity

Message identity can be explicitly specified, overriding the default identity provided by NServiceBus.

WARNING: It is important that the strategy used to generate message identities results in globally unique identifiers. If two messages ever have the same identity then some features will treat them as the same message. This will cause errors which are difficult to diagnose.

In Version 5 and below, message identity can be set by manipulating the `NServiceBus.MessageId` header in an [Outgoing Message Mutator](/nservicebus/pipeline/message-mutators.md).

snippet: MessageId-Mutator

WARNING: Setting the `NServiceBus.MessageId` header from a message mutator is not supported in Version 6 and above. Specify message identity using the `SendOptions` class.

snippet: MessageId-SendOptions