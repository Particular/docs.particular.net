---
title: Bond Serializer
component: Bond
reviewed: 2016-11-07
related:
 - samples/serializers/bond
---

Serializes messages with [Microsoft Bond](https://microsoft.github.io/bond/manual/bond_cs.html).


## Usage

snippet:BondSerialization



### SerializationDelegates

Customizes the cached delegate that serialize and deserialize message types. This is an optional setting.

The default serialization delegates are as follows.

snippet: BondSerializationDelegates




include: custom-contenttype-key

snippet:BondContentTypeKey