---
title: MessagePack Serializer
component: MessagePack
reviewed: 2017-09-30
related:
 - samples/serializers/messagepack
 - nservicebus/serialization/msgpack
redirects:
 - nservicebus/serialization/message-pack
---

Serialize messages with the [MessagePack](http://msgpack.org/) binary format via the [MessagePack-CSharp](https://github.com/neuecc/MessagePack-CSharp) project.


## Usage

snippet: MessagePackSerialization

include: interface-not-supported


### Resolver

Customizes the instance of `IFormatterResolver` used for serialization.

snippet: MessagePackResolver


include: custom-contenttype-key

snippet: MessagePackContentTypeKey