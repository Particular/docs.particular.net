---
title: Custom serializers
summary: How to create and register custom serializer for NServiceBus.
component: Core
versions: '[5,)'
reviewed: 2018-03-29
tags:
- serialization
redirects:
- nservicebus/how-to-register-a-custom-serializer
---


### Create a custom serializer

A custom serializer needs to implement `IMessageSerializer` interface:

snippet: CustomSerializer

In order to see sample implementations, refer to the [community run serializers](/components#serializers).


### Register the serializer

To use the customer serializer it must be registered as part of the endpoint configuration:

snippet: RegisterCustomSerializer

Note: When using multiple deserializers make sure that there's only one type registered per given `ContentType`.
