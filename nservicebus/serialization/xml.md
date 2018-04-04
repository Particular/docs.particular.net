---
title: XML Serializer
summary: A custom XML serializer
reviewed: 2018-04-04
component: Xml
redirects:
 - nservicebus/serialization/xml-serializer
related:
 - samples/serializers/xml
---

## Usage

snippet: XmlSerialization

## Inferring message type from root node name

In integration scenarios where the sender is unable to add headers to the message, the serializer can infer the CLR message type based on the root node of the XML payload. To take advantage of this, the name of the payload's root node must match the [`Type.FullName`](https://msdn.microsoft.com/en-us/library/system.type.fullname) of a message type known to the receiving endpoint.

This technique allows messages without any headers, including the `NServiceBus.EnclosedMessageTypes` header, to be processed. This is demonstrated by the [native integration with RabbitMQ sample](/samples/rabbitmq/native-integration/).

partial: raw


## Caveats


### XML attributes

The XML serializer in NServiceBus is a custom implementation. As such it does not support the [standard .NET XML attributes](https://docs.microsoft.com/en-us/dotnet/framework/serialization/controlling-xml-serialization-using-attributes).


### Unsupported types

 * [HashTable](https://msdn.microsoft.com/en-us/library/system.collections.hashtable.aspx)
 * Types with non-default constructors. So types must have a public constructor with no parameters.
 * [ArrayList](https://msdn.microsoft.com/en-us/library/system.collections.arraylist.aspx)
