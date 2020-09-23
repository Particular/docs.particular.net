---
title: Serialization
summary: Information about how messages are serialized and deserialized on a transport
component: Core
reviewed: 2019-08-12
isLearningPath: true
related:
 - samples/pipeline/multi-serializer
 - samples/serializers
---

NServiceBus takes instances of .NET objects (messages, events, and commands) and sends/receives them over a specified [transport](/transports/). As part of this process, the object must be serialized and deserialized. NServiceBus achieves this using **serializers**.

NOTE: When transitioning to a new serializer, messages that are currently 'in-flight' are formatted using the previous serialization format. **This includes [saga timeout](/nservicebus/sagas/timeouts.md) and [deferred/delayed](/nservicebus/messaging/delayed-delivery.md) messages via timeout persistence.** 

partial: livemigration


The [Newtonsoft JSON Serializer](newtonsoft.md) provides an effective general-purpose serializer appropriate for most use cases based on the ubiquitous [Json.NET package](https://www.newtonsoft.com/json). The Newtonsoft package provides a good combination of compactness, human readability, and performance. Other serializers are supported in order to enable specific performance or integration requirements.

## Supported serializers

 * [Newtonsoft](newtonsoft.md)
 * [XML](xml.md)
 * [JSON/BSON](json.md) (deprecated in NServiceBus versions 7 and above)

## Configuring a serializer

A serializer can be configured using the `endpointConfiguration.UseSerialization` API. Refer to the dedicated documentation pages for each available serializer for more information about the specific configuration.

NOTE: The same serializer must be used by the sending endpoint to serialize messages and by the receiving endpoint to deserialize them, unless additional deserializers are specified.


## Using the default serializer

The default serializer used in NServiceBus projects is the custom [XmlSerializer](xml.md). Unless explicitly configured otherwise, NServiceBus will use [XmlSerializer](xml.md) for serializing and deserializing all messages.


## Using a custom serializer

Besides the officially supported and community maintained serializers, it is also possible to [implement and register a custom serializer](/nservicebus/serialization/custom-serializer.md#register-the-serializer).


partial: additionaldeserializers

## Immutable message types

It is possible to [use immutable types as messages](/nservicebus/messaging/immutable-messages.md). NServiceBus does not restrict this; It depends on the chosen serializer implementation if it supports deserializing to non public properties and/or using non-default constructors to initialize types.

NOTE: On the wire it makes no difference if mutable or immutable message types are used.

For example, the [Newtonsoft JSON Serializer](newtonsoft.md) by default supports immutable messages types.
