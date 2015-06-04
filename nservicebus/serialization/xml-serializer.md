---
title: Xml Serializer
summary: A custom written XML serializer.
---

A custom written XML serializer.


## Usage

<!-- import XmlSerialization --> 


## Caveats


### XML Attributes

The XML serializer in NServiceBus is a custom implementation. As such it does not support the [standard .net XML Attributes](https://msdn.microsoft.com/en-us/library/2baksw0z.aspx).


### Unsupported Types

 * HashTable
 * Types with non-default constructors. So types must have a public constructor with no parameters.
 * ArrayList