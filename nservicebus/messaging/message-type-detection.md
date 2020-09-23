---
title: Message type detection
summary: Describes the rules for mapping a transport message to a message type
reviewed: 2019-05-31
component: Core
---

In order to invoke the correct message handlers for incoming messages, NServiceBus must be able to map the incoming transport message to a [message type](/nservicebus/messaging/messages-events-commands.md).

The mapping rules are as follows:

1. If the message contains the [`NServiceBus.EnclosedMessageTypes` header](/nservicebus/messaging/headers.md#serialization-headers-nservicebus-enclosedmessagetypes), the header value is used to find the message type. The header value may contain:
   - The [FullName](https://docs.microsoft.com/en-us/dotnet/api/system.type.fullname) of the message type.
   - The [AssemblyQualifiedName](https://docs.microsoft.com/en-us/dotnet/api/system.type.assemblyqualifiedname) of the message type (with or without private key are both supported)
   - The Name of the type, without the assembly name.
   
NServiceBus uses the AssemblyQualifiedName when emitting messages. 

1. If the header is missing, serializers can optionally infer the message type based on the message payload.

## Serializers that support message type inference

* [XML](/nservicebus/serialization/xml.md#inferring-message-type-from-root-node-name) via the root node name
* [JSON.NET](/nservicebus/serialization/newtonsoft.md#inferring-message-type-from-type) via a custom `$type` property
