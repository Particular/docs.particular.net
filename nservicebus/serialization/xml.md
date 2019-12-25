---
title: XML Serializer
summary: A custom XML serializer
reviewed: 2019-12-25
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


## Raw XML

In certain integration scenarios it may be necessary to bypass NServiceBus's opinionated serialization format (essentially key/value pairs) and directly send custom XML structures over messaging. In order to do that, declare one or more properties on the message contract as `XDocument` or `XElement`.


### Message with XDocument

snippet: MessageWithXDocument


### Payload with XDocument

snippet: XDocumentPayload


### Message with XElement

snippet: MessageWithXElement


### Payload with XElement

snippet: XElementPayload


The caveat of this approach is that the serializer will wrap the data in an outer node named after the name of the property. In the examples above, note the associated expected payloads.

To avoid that, for interoperability reasons, instruct the serializer not to wrap raw XML structures:

snippet: ConfigureRawXmlSerialization

NOTE: The name of the property on the message must match the name of the root node in the XML structure exactly in order to be able to correctly deserialize the no longer wrapped content.


## Caveats


### XML attributes

The XML serializer in NServiceBus is a custom implementation. As such it does not support the [standard .NET XML attributes](https://docs.microsoft.com/en-us/dotnet/framework/serialization/controlling-xml-serialization-using-attributes).


### Unsupported types

 * [HashTable](https://msdn.microsoft.com/en-us/library/system.collections.hashtable.aspx)
 * Types with non-default constructors. So types must have a public constructor with no parameters.
 * [ArrayList](https://msdn.microsoft.com/en-us/library/system.collections.arraylist.aspx)
