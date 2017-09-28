---
title: Message Body Encryption
component: Core
reviewed: 2016-11-16
tags:
- Encryption
- Security
related:
- samples/encryption
- nservicebus/security
---


Message encryption leverages the pipeline to encrypt the entire serialized message body.

One way of achieving this is by using a [transport message mutator](/nservicebus/pipeline/message-mutators.md#transport-messages-mutators).

snippet: MessageBodyEncryptor

The encrypting mutator must be injected into dependency injection:

snippet: UsingMessageBodyEncryptor
