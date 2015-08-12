---
title: Serialization In NServiceBus
summary: How instances of .net classes are serialized onto the transport.
---

NServiceBus takes instances of .net objects (messages, events and commands) and then sens/receives them over a specified [Transport](/nservicebus/transports/). As part of this the object need to be serialized and deserialized. To achieve this NServiceBus uses "Serializers"


### Community run Serializers

There are several community run Serializers that can be seen on the full list of [Extensions](/platform/extensions.md#serializers).
