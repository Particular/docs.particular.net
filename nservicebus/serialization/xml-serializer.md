---
title: Xml Serializer
summary: A custom written XML serializer.
---

A custom written XML serializer.


## Usage

<!-- import XmlSerialization --> 


## Raw XML
In certain integration scenarios you might want to bypass NServiceBus opinionated serialization format (essentially key/value pairs) and directly send custom XML structures over messaging. If you do need to do so you can bypass serialization on an endpoint-per-endpoint base by configuring the XML serializer like the following

<!-- import ConfigureRawXmlSerialization --> 

TODO: continue explain message definition and sending messages

## Caveats


### XML Attributes

The XML serializer in NServiceBus is a custom implementation. As such it does not support the [standard .net XML Attributes](https://msdn.microsoft.com/en-us/library/2baksw0z.aspx).


### Unsupported Types

 * HashTable
 * Types with non-default constructors. So types must have a public constructor with no parameters.
 * ArrayList