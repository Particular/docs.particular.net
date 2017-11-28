---
title: Serialization
summary: .NET classes are serialized onto the transport.
component: Core
reviewed: 2016-11-07
related:
 - samples/pipeline/multi-serializer
 - samples/serializers
---

NServiceBus takes instances of .NET objects (messages, events and commands) and then sends/receives them over a specified [Transport](/transports/). As part of this the object needs to be serialized and deserialized. To achieve this NServiceBus uses **Serializers**.

NOTE: When transitioning to a new serializer then messages that are currently 'in flight' are formatted using the previous serialization format. **This includes [Saga Timeout](/nservicebus/sagas/timeouts.md) and [Deferred/Delayed](/nservicebus/messaging/delayed-delivery.md) messages via timeout persistence.** 

partial: livemigration


The [Newtonsoft JSON Serializer](newtonsoft.md) provides an effective general-purpose serializer appropriate for most use cases based on the ubiquitous [Json.NET package](http://www.newtonsoft.com/json). The Newtonsoft package provides a good combination of compactness, human readability, and performance. Other serializers are supported in order to enable specific performance or integration requirements.


### Supported Serializers

 * [Newtonsoft](newtonsoft.md)
 * [Xml](xml.md)
 * [ProtoBuf-Net](protobufnet.md)
 * [ProtoBuf-Google](protobufgoogle.md)
 * [MessagePack](messagepack.md)
 * [MsgPack](msgpack.md)
 * [Wire](wire.md)
 * [Hyperion](hyperion.md)
 * [Jil](jil.md)
 * [Utf8Json](utf8json.md)
 * [Bond](bond.md)
 * [ZeroFormatter](zeroformatter.md)
 * [JSON/BSON](json.md) (deprecated in Versions 7 and above)
 * [Binary](binary.md) (deprecated in Versions 6 and above)


### Using an existing serializer

The default serializer used in NServiceBus projects is the custom [XmlSerializer](xml.md).

The pages dedicated to particular Serializers show how to configure the endpoint to use each of them. Unless explicitly configured otherwise, NServiceBus will use [XmlSerializer](xml.md) for serializing and deserializing all messages.

NOTE: The same Serializer must be used by the sending endpoint to serialize messages and by receiving endpoint to deserialize them, unless additional deserializers are specified.

In order to register community run serializer or custom serializer, refer to the [Custom serializers - Register the serializer](/nservicebus/serialization/custom-serializer.md#register-the-serializer) section.


partial: additionaldeserializers
