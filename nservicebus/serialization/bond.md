---
title: Bond Serializer
summary: Serialization and deserialization with Microsoft Bond using the NServiceBus.Bond community package
component: Bond
reviewed: 2018-09-21
related:
 - samples/serializers/bond
---

Serializes messages with [Microsoft Bond](https://microsoft.github.io/bond/manual/bond_cs.html).

> Bond is a cross-platform framework for working with schematized data. It supports cross-language serialization/deserialization and powerful generic mechanisms for efficiently manipulating data. Bond is broadly used at Microsoft in high-scale services.


## Usage

snippet: BondSerialization

include: interface-not-supported


### SerializationDelegates

Customizes the cached delegates that serialize and deserialize message types. This is an optional setting.

The default serialization delegates are as follows.

snippet: BondSerializationDelegates

The serializers are cached as per the [Bond performance guidance](https://microsoft.github.io/bond/manual/bond_cs.html#performance).

snippet: SerializerCache


include: custom-contenttype-key

snippet: BondContentTypeKey
