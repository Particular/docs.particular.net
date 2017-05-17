---
title: ProtocolBuf-Google Serializer
component: ProtoBufGoogle
reviewed: 2017-05-17
related:
 - samples/serializers/protobufgoogle
 - nservicebus/serialization/protobufnet
---

Serializes messages using [Google Protocol Buffers](https://developers.google.com/protocol-buffers/docs/reference/csharp-generated).

include: protobufmultiple


## Usage

snippet: ProtobufSerialization

include: interface-not-supported


### Custom Settings

Customizes the instance of `SerializerOptions` used for serialization.

include: custom-contenttype-key

snippet: ProtoBufContentTypeKey

include: protobufgoogleinfo