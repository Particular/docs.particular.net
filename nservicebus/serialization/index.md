---
title: Serialization In NServiceBus
summary: How instances of .NET classes are serialized onto the transport.
---

NServiceBus takes instances of .NET objects (messages, events and commands) and then sends/receives them over a specified [Transport](/nservicebus/transports/). As part of this the object need to be serialized and deserialized. To achieve this NServiceBus uses **Serializers**.

## Supported Serializers
- [XmlSerializer](xml.md)
- [JsonSerializer](json.md)
- [BinarySerializer](binary.md)

### Community run Serializers

There are several community run Serializers that can be seen on the full list of [Extensions](/platform/extensions.md#serializers).

## Using an existing Serializer

The default Serializer used in NServiceBus projects is the custom [XmlSerializer](xml.md). 
 
The pages dedicated to particular Serializers show how to configure the endpoint to use each of them. Unless explicitly configured otherwise, NServiceBus will use XmlSerializer for serializing and deserializing all messages.

Note that the same Serializer must be used by the sending endpoint to serialize messages and by receiving endpoint to deserialize them. 
