---
title: Serialization
summary: .NET messaging systems require serialization and deserialization of objects sent/received over transports. NServiceBus achieves this using serializers.
component: Core
reviewed: 2025-06-21
isLearningPath: true
related:
 - samples/serializers
---

NServiceBus takes instances of .NET objects (messages, events, and commands) and sends/receives them over a specified [transport](/transports/). As part of this process, the object must be serialized and deserialized. NServiceBus achieves this using **serializers**.

It's possible to transition to another serialization format while still remaining compatible with messages in-flight that used the previous serialization format. This is accomplished by adding the previous serialization format as an [additional deserializer](#specifying-additional-deserializers), which is supported in NServiceBus versions 6 and above. This way, messages serialized before the transition (including [saga timeouts](/nservicebus/sagas/timeouts.md) and [delayed messages](/nservicebus/messaging/delayed-delivery.md)) can still be understood while new messages use the new serializer.

The [System.Text.Json serializer](system-json.md) provides an effective general-purpose serializer appropriate for most use cases based on the [JSON serialization built into .NET](https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/how-to). JSON provides a good combination of compactness, human readability, and performance.

Other serializers are supported in order to enable specific performance or integration requirements.

## Supported serializers

* [System.Text.Json](system-json.md)
* [Newtonsoft](newtonsoft.md)
* [XML](xml.md)

partial: configuring

## Using a custom serializer

Besides the officially supported and community maintained serializers, it is also possible to [implement and register a custom serializer](/nservicebus/serialization/custom-serializer.md).


## Specifying additional deserializers

To support sending and receiving messages between endpoints using different serializers, additional deserialization capability may be specified. It is possible to register additional deserializers to process incoming messages. Additionally, if a deserializer requires custom settings, they can be provided during its registration.

snippet: AdditionalDeserializers

> [!NOTE]
> When using multiple deserializers make sure that there's only one type registered per given `ContentType`.

## Immutable message types

It is possible to [use immutable types as messages](/nservicebus/messaging/immutable-messages.md). NServiceBus does not restrict this; It depends on the chosen serializer implementation if it supports deserializing to non public properties and/or using non-default constructors to initialize types.

When messages are in serialized form "on the wire" it makes no difference if mutable or immutable message types are used.

The [System.Text.Json serializer](system-json.md) and [Newtonsoft JSON Serializer](newtonsoft.md) by default support immutable messages types.

## Security

The deserialization target type is defined by the incoming message. Although NServiceBus only deserializes message payloads that are considered a [valid message type](/nservicebus/messaging/messages-events-commands.md), side effects in constructor methods or property setters of message contracts may be abused by an attacker with access to the transport infrastructure.

To avoid unintended behavior during message deserialization, avoid executing code with side effects as part of constructors and property setters of message types.

partial: security
