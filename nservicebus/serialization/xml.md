---
title: Xml Serializer
summary: A custom written XML serializer.
reviewed: 2016-08-23
component: Xml
redirects:
 - nservicebus/serialization/xml-serializer
related:
 - samples/serializers/xml
---

A custom written XML serializer.


## Usage

snippet: XmlSerialization

## Infering message type from root node name

In integration scenarios where the sender is unable add headers to the message the serializer can infer the CLR message type based on the root node of the XML payload. This convention is that the `{xml root node name}` must match the [`Type.FullName`](https://msdn.microsoft.com/en-us/library/system.type.fullname) of a message type known to the receiving endpoint. 

Using this technique messages without any headers, the `NServiceBus.EnclosedMessageTypes` header specifically, can be processed. This is demonstrated by the [native integration with RabbitMQ sample](/samples/rabbitmq/native-integration/).

partial: raw


## Caveats


### XML Attributes

The XML serializer in NServiceBus is a custom implementation. As such it does not support the [standard .NET XML Attributes](https://docs.microsoft.com/en-us/dotnet/framework/serialization/controlling-xml-serialization-using-attributes).


### Unsupported Types

 * [HashTable](https://msdn.microsoft.com/en-us/library/system.collections.hashtable.aspx)
 * Types with non-default constructors. So types must have a public constructor with no parameters.
 * [ArrayList](https://msdn.microsoft.com/en-us/library/system.collections.arraylist.aspx)
