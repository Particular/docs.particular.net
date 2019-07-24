---
title: MsgPack Serializer
component: MsgPack
reviewed: 2019-07-08
related:
 - samples/serializers/msgpack
 - nservicebus/serialization/messagepack
 - nservicebus/upgrades/messagepack-changes
---

Serialize messages with the [MessagePack](https://msgpack.org/) binary format via the [MsgPack.Cli](https://github.com/msgpack/msgpack-cli) project.


## Usage

snippet: MsgPackSerialization

include: interface-not-supported


### Custom Settings

Customizes the instance of `SerializerOptions` used for serialization.

snippet: MsgPackCustomSettings


include: custom-contenttype-key

snippet: MsgPackContentTypeKey