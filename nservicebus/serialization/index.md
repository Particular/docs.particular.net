---
title: Serialization In NServiceBus
summary: How instances of .net classes are serialized onto the transport.
---

NServiceBus takes instances of .net objects (messages, events and commands) and then sens/receives them over a specified [Transport](/nservicebus/transports/). As part of this the object need to be serialized and deserialized. To achieve this NServiceBus uses "Serializers"

## Available Serializers

### [Xml](xml-serializer.md)

A custom written XML serializer.

#### Usage

<!-- import XmlSerialization --> 

### Json

Using [Json](http://en.wikipedia.org/wiki/Json) via an ILMeged copy of Json.NET.

#### Usage

<!-- import JsonSerialization -->

### Bson

Using [Bson](http://en.wikipedia.org/wiki/BSON) via an ILMeged copy of Json.NET.

#### Usage

<!-- import BsonSerialization -->

### Binary

Uses the .net [BinaryFormatter](https://msdn.microsoft.com/en-us/library/system.runtime.serialization.formatters.binary.binaryformatter.aspx).

#### Usage

<!-- import BinarySerialization -->

### Community run Serializers

There are several community run Serializers that can be seen on the full list of [Extensions](/platform/extensions.md#serializers).