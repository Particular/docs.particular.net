---
title: Message Identity
summary: How are individual messages identified?
---

Each message that is dispatched from an [Endpoint](/nservicebus/endpoints/) has a unique identity. NServiceBus generates and stores this value as a [header](/nservicebus/messaging/headers) on the message (`NServiceBus.MessageId`). If the selected transport supports message identity, this value is used as the transport level message identity as well. 

Many features take advantage of message identity. For example, the [Outbox](/nservicebus/outbox) uses message identity to deduplicate messages and [First Level Retries](/nservicebus/errors/automatic-retries) uses message identity to keep track of how many times the endpoint has tried to process a message.

## Specifying message identity

Message identity can be explicitly specified, overriding the default identity provided by NServiceBus. 

WARN: It is important that the strategy used to generate message identities results in globally unique identifiers. If two messages ever have the same identity then some features will treat them as the same message. This will cause errors which are difficult to diagnose.

Message identity can be set by manipulating the `NServiceBus.MessageId` header in an [Outgoing Message Mutator](/nservicebus/pipeline/message-mutators). 

snippet: MessageId-Mutator

In Version 6 and above, message identity can also be set using the `SendOptions` class.

snippet: MessageId-SendOptions

