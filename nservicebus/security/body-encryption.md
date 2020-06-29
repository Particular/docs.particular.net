---
title: Message Body Encryption
summary: Storing message bodies fully encrypted in the queueing infrastructure
component: Core
reviewed: 2020-06-29
related:
- samples/encryption
- nservicebus/security
---


Message encryption leverages the pipeline to encrypt the entire serialized message body.

Note: Data in the message headers is not encrypted.

One way of achieving this is by using a [transport message mutator](/nservicebus/pipeline/message-mutators.md#transport-messages-mutators).

snippet: MessageBodyEncryptor

The encrypting mutator must be injected via dependency injection:

snippet: UsingMessageBodyEncryptor
