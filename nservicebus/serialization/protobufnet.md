---
title: ProtoBuf-Net Serializer
component: ProtoBufNet
reviewed: 2019-03-01
related:
 - samples/serializers/protobufnet
 - nservicebus/serialization/protobufgoogle
---

Serializes messages using [protobuf-net](https://github.com/mgravell/protobuf-net).

include: protobufmultiple

## Usage

snippet: ProtobufSerialization

include: interface-not-supported


### Custom Settings

Customizes the `SerializerOptions` used for serialization.

snippet: ProtoBufCustomSettings


include: custom-contenttype-key

snippet: ProtoBufContentTypeKey
