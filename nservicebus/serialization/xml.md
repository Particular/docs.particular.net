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


partial: raw


## Caveats


### XML Attributes

The XML serializer in NServiceBus is a custom implementation. As such it does not support the [standard .NET XML Attributes](https://docs.microsoft.com/en-us/dotnet/framework/serialization/controlling-xml-serialization-using-attributes).


### Unsupported Types

 * [HashTable](https://msdn.microsoft.com/en-us/library/system.collections.hashtable.aspx)
 * Types with non-default constructors. So types must have a public constructor with no parameters.
 * [ArrayList](https://msdn.microsoft.com/en-us/library/system.collections.arraylist.aspx)