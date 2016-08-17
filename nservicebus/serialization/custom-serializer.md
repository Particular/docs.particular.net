---
title: Custom serializers
summary: How to create and register custom serializer for NServiceBus.
component: Core
versions: [5,)
reviewed: 2016-08-17
tags:
- serialization
redirects:
- nservicebus/how-to-register-a-custom-serializer
---


### Create a custom serializer

A custom serializer needs to implement `IMessageSerializer` interface:

snippet: CustomSerializer

In order to see sample implementations, refer to the [community run serializers](/platform/extensions.md#serializers).


### Register the serializer

Register the serializer:

snippet:RegisterCustomSerializer

Note: When using multiple deserializers make sure that there's only one type registered per given `ContentType`.