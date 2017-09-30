---
title: MsgPack Serializer
component: MsgPack
reviewed: 2017-09-30
related:
 - samples/serializers/msgpack
 - nservicebus/serialization/messagepack
redirects:
 - nservicebus/serialization/message-pack
---

Serialize messages with the [MessagePack](http://msgpack.org/) binary format via the [MsgPack.Cli](https://github.com/msgpack/msgpack-cli) project.


## Usage

snippet: MsgPackSerialization

include: interface-not-supported


### Custom Settings

Customizes the instance of `SerializerOptions` used for serialization.

snippet: MsgPackCustomSettings


include: custom-contenttype-key

snippet: MsgPackContentTypeKey