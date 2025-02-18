---
title: Multiple Deserializers
summary: Using the AddDeserializer API to enable deserializing multiple formats.
reviewed: 2025-01-16
component: Core
related:
 - nservicebus/serialization
---

This sample uses the `AddDeserializer` API to illustrate a receiving endpoint deserializing multiple serialization formats.

## Sending endpoints

There are multiple sending endpoints, one per serializer.

### NewtonsoftJsonEndpoint

Sends messages using the external [Json.NET serializer](/nservicebus/serialization/newtonsoft.md) in JSON format.

snippet: configExternalNewtonsoftJson

### NewtonsoftBsonEndpoint

Sends messages using the external [Json.NET serializer](/nservicebus/serialization/newtonsoft.md) in BSON format.

snippet: configExternalNewtonsoftBson

### XmlEndpoint

Sends messages using the [XML serializer](/nservicebus/serialization/xml.md).

snippet: configXml

partial: jsonserializer

## Shared

Contains message definitions and a [message mutator](/nservicebus/pipeline/message-mutators.md) that logs the outgoing data. The project is shared by all endpoints and configured by `endpointConfiguration.RegisterOutgoingMessageLogger();` in the above snippets.

snippet: outgoingmutator

## ReceivingEndpoint

The receiving endpoint references all the serializers used above and adds them using `AddDeserializer`

snippet: configAll

There is also a [message mutator](/nservicebus/pipeline/message-mutators.md) that logs the incoming data.

snippet: incomingmutator
