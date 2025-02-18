---
title: XML Serializer
summary: A custom XML serializer
reviewed: 2025-01-17
component: Xml
redirects:
 - nservicebus/serialization/xml-serializer
related:
 - samples/serializers/xml
---

## Usage

snippet: XmlSerialization

## Inferring message type from root node name

In integration scenarios where the sender is unable to add headers to the message, the serializer can infer the message type based on the root node of the XML payload. To take advantage of this, the name of the payload's root node must match the [`Type.FullName`](https://msdn.microsoft.com/en-us/library/system.type.fullname) of a message type known to the receiving endpoint.

This technique allows messages without any headers, including the `NServiceBus.EnclosedMessageTypes` header, to be processed. This is demonstrated by the [native integration with RabbitMQ sample](/samples/rabbitmq/native-integration/).

## Raw XML

In certain integration scenarios it may be necessary to bypass NServiceBus's opinionated serialization format (essentially key/value pairs) and directly send custom XML structures over messaging. In order to do that, declare one or more properties on the message contract as `XDocument` or `XElement`.

### Message with XDocument

snippet: MessageWithXDocument

### Message with XElement

snippet: MessageWithXElement

### Payload with raw XML

Using `XDocument` or `XElement` message properties will wrap the raw XML data in an outer node named after the property. The node will have the message definitions above and the following XML content:

snippet: RawXmlContent

The full message payload might look like this:

snippet: RawXmlPayload

To avoid the extra node (e.g. for interoperability reasons), instruct the serializer not to wrap raw XML structures:

snippet: ConfigureRawXmlSerialization

This will change the payload as follows:

snippet: RawXmlNoWrapPayload

> [!NOTE]
> The name of the property on the message must match the name of the root node in the XML structure exactly in order to be able to correctly deserialize the no longer wrapped content.

## Caveats

### XML attributes

The XML serializer in NServiceBus is a custom implementation. As such it does not support the [standard .NET XML attributes](https://docs.microsoft.com/en-us/dotnet/framework/serialization/controlling-xml-serialization-using-attributes).

### Unsupported types

* Types with non-default constructors. Types must have a public constructor with no parameters.
* [ArrayList](https://msdn.microsoft.com/en-us/library/system.collections.arraylist.aspx)
* [HashTable](https://msdn.microsoft.com/en-us/library/system.collections.hashtable.aspx)
* [DateOnly](https://learn.microsoft.com/en-us/dotnet/api/system.dateonly)
* [TimeOnly](https://learn.microsoft.com/en-us/dotnet/api/system.timeonly)
