---
title: Message type detection
summary: Describes the rules for mapping a transport message to a message type
reviewed: 2023-08-04
component: Core
related:
- samples/consumer-driven-contracts
---

NServiceBus sets the [`NServiceBus.EnclosedMessageTypes` header](/nservicebus/messaging/headers.md#serialization-headers-nservicebus-enclosedmessagetypes) to the [AssemblyQualifiedName](https://docs.microsoft.com/en-us/dotnet/api/system.type.assemblyqualifiedname) of the emitted message type when sending, publishing or replying.

It is possible to change the enclosed message type header for all emitted messages by using a behavior. The [consumder-driven-contracts sample](/samples/consumer-driven-contracts) shows how to manipulate the header for outgoing messages.

To invoke the correct message handlers for incoming messages, NServiceBus must be able to map the incoming transport message to a [message type](/nservicebus/messaging/messages-events-commands.md).

The mapping rules are as follows:

1. If the message contains the [`NServiceBus.EnclosedMessageTypes` header](/nservicebus/messaging/headers.md#serialization-headers-nservicebus-enclosedmessagetypes), the header value is used to find the message type. The header value may contain:
   - The [AssemblyQualifiedName](https://docs.microsoft.com/en-us/dotnet/api/system.type.assemblyqualifiedname) of the message type (with or without private key are both supported). In cases when the assembly qualified type is not known by the endpoint NServiceBus will fall back using the [FullName](https://docs.microsoft.com/en-us/dotnet/api/system.type.fullname) of the message type.
   - The [FullName](https://docs.microsoft.com/en-us/dotnet/api/system.type.fullname) of the message type.
   - The Name of the type, without the assembly name.
1. If the header is missing, some serializers can optionally [infer the message type](/nservicebus/serialization/index.md#security-message-type-inference) based on the message payload. Serializers that support message type inference:
   - [XML](/nservicebus/serialization/xml.md#inferring-message-type-from-root-node-name) via the root node name
   - [JSON.NET](/nservicebus/serialization/newtonsoft.md#inferring-message-type-from-type) via a custom `$type` property
