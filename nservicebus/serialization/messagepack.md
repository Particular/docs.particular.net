---
title: MessagePack Serializer
component: MessagePack
reviewed: 2017-09-30
related:
 - samples/serializers/messagepack
redirects:
 - nservicebus/serialization/message-pack
---

Serialize messages with the [MessagePack](http://msgpack.org/) binary format via the [MessagePack-CSharp](https://github.com/neuecc/MessagePack-CSharp) project.


## Usage

snippet: MessagePackSerialization

include: interface-not-supported


### Custom Settings

Customizes the instance of `SerializerOptions` used for serialization.

snippet: MessagePackCustomSettings


include: custom-contenttype-key

snippet: MessagePackContentTypeKey