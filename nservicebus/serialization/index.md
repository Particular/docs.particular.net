---
title: Serialization In NServiceBus
summary: How instances of .NET classes are serialized onto the transport.
reviewed: 2016-03-17
related:
 - samples/pipeline/multi-serializer
---

NServiceBus takes instances of .NET objects (messages, events and commands) and then sends/receives them over a specified [Transport](/nservicebus/transports/). As part of this the object need to be serialized and deserialized. To achieve this NServiceBus uses **Serializers**.


### Supported Serializers

 * [XmlSerializer](xml.md)
 * [JsonSerializer/BsonSerializer](json.md)
 * [NewtonsoftSerializer](newtonsoft.md)
 * [BinarySerializer](binary.md) (deprecated in Versions 6 and above)


### Using an existing serializer

The default serializer used in NServiceBus projects is the custom [XmlSerializer](xml.md).

The pages dedicated to particular Serializers show how to configure the endpoint to use each of them. Unless explicitly configured otherwise, NServiceBus will use XmlSerializer for serializing and deserializing all messages.

NOTE: The same Serializer must be used by the sending endpoint to serialize messages and by receiving endpoint to deserialize them, unless additional deserializers are specified.


### Specifying additional deserializers

To support sending and receiving messages between endpoints using different serializers, additional deserialization capability may be specified. Starting from NServiceBus Version 6 it's possible to register additional deserializers to process incoming messages. Additionally, if a deserializer requires custom settings, they can be provided during its registration.

snippet:AdditionalDeserializers


### How to implement your own serializer

To implement a serializer all it needs to be done is implement `IMessageSerializer`.
The only tricky bit is to ensure that the string returned by `ContentType` does not clash with any existing registered serializer, the `ContentType` string is used internally as a dictionary key lookup.


### Community run serializers

There are several community run Serializers that can be seen on the full list of [Extensions](/platform/extensions.md#serializers).
