---
title: Serialization
summary: Information about how messages are serialized and deserialized on a transport
component: Core
reviewed: 2021-09-28
isLearningPath: true
related:
 - samples/pipeline/multi-serializer
 - samples/serializers
---

NServiceBus takes instances of .NET objects (messages, events, and commands) and sends/receives them over a specified [transport](/transports/). As part of this process, the object must be serialized and deserialized. NServiceBus achieves this using **serializers**.

NOTE: When transitioning to a new serializer, messages that are currently 'in-flight' are formatted using the previous serialization format. **This includes [saga timeout](/nservicebus/sagas/timeouts.md) and [deferred/delayed](/nservicebus/messaging/delayed-delivery.md) messages via timeout persistence.**

It's possible to transition to another serialization format while still remaining compatible with messages in-flight that used the previous serialization format. This is accomplished by adding the previous serialization format as an [additional deserializer](#specifying-additional-deserializers), which is supported in NServiceBus versions 6 and above.

The [System.Text.Json serializer](system-json.md) provides an effective general-purpose serializer appropriate for most use cases based on the [JSON serialization built into .NET](https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/how-to). JSON provides a good combination of compactness, human readability, and performance.

Other serializers are supported in order to enable specific performance or integration requirements.

## Supported serializers

* [System.Text.Json](system-json.md)
* [Newtonsoft](newtonsoft.md)
* [XML](xml.md)

## Configuring a serializer

A serializer can be configured using the `endpointConfiguration.UseSerialization` API. Refer to the dedicated documentation pages for each available serializer for more information about the specific configuration.

NOTE: The same serializer must be used by the sending endpoint to serialize messages and by the receiving endpoint to deserialize them, unless additional deserializers are specified.

## Using the default serializer

The default serializer used in NServiceBus projects is the custom [XmlSerializer](xml.md). Unless explicitly configured otherwise, NServiceBus will use [XmlSerializer](xml.md) for serializing and deserializing all messages.

WARN: In NServiceBus 8.1 and above, a runtime warning will encourage explicitly selecting a serializer. In a future version of NServiceBus, the XmlSerializer will no longer be selected by default.

## Using a custom serializer

Besides the officially supported and community maintained serializers, it is also possible to [implement and register a custom serializer](/nservicebus/serialization/custom-serializer.md).


## Specifying additional deserializers

To support sending and receiving messages between endpoints using different serializers, additional deserialization capability may be specified. It is possible to register additional deserializers to process incoming messages. Additionally, if a deserializer requires custom settings, they can be provided during its registration.

snippet: AdditionalDeserializers

Note: When using multiple deserializers make sure that there's only one type registered per given `ContentType`.

## Immutable message types

It is possible to [use immutable types as messages](/nservicebus/messaging/immutable-messages.md). NServiceBus does not restrict this; It depends on the chosen serializer implementation if it supports deserializing to non public properties and/or using non-default constructors to initialize types.

NOTE: On the wire it makes no difference if mutable or immutable message types are used.

For example, the [Newtonsoft JSON Serializer](newtonsoft.md) by default supports immutable messages types.

## Security

The deserialization target type is defined by the incoming message. Although NServiceBus only deserializes message payloads that are considered a [valid message type](/nservicebus/messaging/messages-events-commands.md), side effects in constructor methods or property setters of message contracts may be abused by an attacker with access to the transport infrastructure.

To avoid unintended behavior during message deserialization, avoid executing code with side effects as part of constructors and property setters of message types.

partial: security
