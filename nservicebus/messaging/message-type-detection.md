---
title: Message type detection
reviewed:
component: Core
---

In order to invoke the correct message handlers for incoming messages NServiceBus needs to be able to map the incoming message to a [message type](/nservicebus/messaging/messages-events-commands.md).

The mapping rules are as follows:

1. If the message contains the [`NServiceBus.EnclosedMessageType` header](/nservicebus/messaging/headers.md##serialization-headers-nservicebus-enclosedmessagetypes) the header value will be used to find the message type.

1. If the header is missing the serializer can optionally infer the message type based on the message payload.


## Serializers supporting message type inferral

* [Xml Serializer](/nservicebus/serialization/xml.md#inferring-message-type-from-root-node-name) can infer message type based on the root node name.
* [JSON.net Serializer](/nservicebus/serialization/newtonsoft.md#inferring-message-type-from-type) can infer message type based on a custom `$type` property.