---
title: Xml Serializer
summary: A custom written XML serializer.
reviewed: 2016-03-17
redirects:
 - nservicebus/serialization/xml-serializer
related:
 - samples/serializers/xml
---

A custom written XML serializer.


## Usage

snippet: XmlSerialization


## Raw XML

In certain integration scenarios it may be necessary to bypass NServiceBus opinionated serialization format (essentially key/value pairs) and directly send custom XML structures over messaging. In order to do that declare one or multiple properties on the message contract as `XDocument` or `XElement`.


### Message with XDocument

snippet: MessageWithXDocument


### Payload with XDocument

snippet: XDocumentPayload


### Message with XElement

snippet: MessageWithXElement


### Payload with XElement

snippet: XElementPayload


The caveat of this approach is that the serializer will wrap the data in an outer node being named after the name of the property. So in the examples above note the associated expected payloads.

To avoid that, for interoperability reasons, instruct the serializer to not wrap raw xml structures:

snippet: ConfigureRawXmlSerialization

NOTE: The name of the property on the message must exactly match the name of the root node in the xml structure in order to be able to correctly deserialize the no longer wrapped content.


## Caveats


### XML Attributes

The XML serializer in NServiceBus is a custom implementation. As such it does not support the [standard .NET XML Attributes](https://msdn.microsoft.com/en-us/library/2baksw0z.aspx).


### Unsupported Types

 * [HashTable](https://msdn.microsoft.com/en-us/library/system.collections.hashtable.aspx)
 * Types with non-default constructors. So types must have a public constructor with no parameters.
 * [ArrayList](https://msdn.microsoft.com/en-us/library/system.collections.arraylist.aspx)