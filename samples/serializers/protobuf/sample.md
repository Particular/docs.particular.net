---
title: ProtoBuf Serializer
summary: Using the ProtoBuf serializer in an endpoint.
reviewed: 2016-10-15
component: ProtoBuf
related:
- nservicebus/serialization
---


## NServiceBus.ProtoBuf

This sample uses the community run serializer [NServiceBus.ProtoBuf](https://github.com/SimonCropp/NServiceBus.ProtoBuf) to serialize messages using [protobuf-net](https://github.com/mgravell/protobuf-net).


## Configuring to use ProtoBuf

snippet: config


## The message definition

snippet: message


## The message send

snippet: messagesend