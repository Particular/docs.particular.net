---
title: Message type detection
reviewed:
component: Core
---

In order to invoke the correct message handlers for incoming messages, NServiceBus needs to be able to map the incoming transport message to a [message type](/nservicebus/messaging/messages-events-commands.md).

The mapping rules are as follows:

1. If the message contains the [`NServiceBus.EnclosedMessageTypes` header](/nservicebus/messaging/headers.md#serialization-headers-nservicebus-enclosedmessagetypes), the header value will be used to find the message type. The header value must at least contain the [FullName](https://docs.microsoft.com/en-us/dotnet/api/system.type.fullname) of the message type but can also contain the [AssemblyQualifiedName](https://docs.microsoft.com/en-us/dotnet/api/system.type.assemblyqualifiedname). NServiceBus uses the AssemblyQualifiedName when emitting messages.

1. If the header is missing, serializers can optionally infer the message type based on the message payload.

## Serializers that support message type inference

* [Xml](/nservicebus/serialization/xml.md#inferring-message-type-from-root-node-name) via root node name
* [Json.NET](/nservicebus/serialization/newtonsoft.md#inferring-message-type-from-type) via custom `$type` property
