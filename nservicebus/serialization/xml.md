---
title: Xml Serializer
summary: A custom written XML serializer.
redirects:
 - nservicebus/serialization/xml-serializer
related:
 - samples/serializers/xml
---

A custom written XML serializer.


## Usage

<!-- import XmlSerialization -->


## Raw XML

In certain integration scenarios you might want to bypass NServiceBus opinionated serialization format (essentially key/value pairs) and directly send custom XML structures over messaging. In order to do that you can simply declare one or multiple properties on your message contract as `XDocument` or `XElement`.

snippet:MessageWithXDocument

snippet:MessageWithXElement

The caveat of this approach is that the serializer will wrap your data in an outer node being named after the name of the property. So in the examples above you can see the associated expected payloads.

If you would like to avoid that for interoperability reasons you need to instruct the serializer to not wrap raw xml structures like the following

<!-- import ConfigureRawXmlSerialization -->

INFO: The name of the property on the message must exactly match the name of the root node in the xml structure in order to be able to correctly deserialize the no longer wrapped content.


## Caveats


### XML Attributes

The XML serializer in NServiceBus is a custom implementation. As such it does not support the [standard .NET XML Attributes](https://msdn.microsoft.com/en-us/library/2baksw0z.aspx).


### Unsupported Types

 * [HashTable](https://msdn.microsoft.com/en-us/library/system.collections.hashtable.aspx)
 * Types with non-default constructors. So types must have a public constructor with no parameters.
 * [ArrayList](https://msdn.microsoft.com/en-us/library/system.collections.arraylist.aspx)
