---
title: Multiple De-Serializers
summary: Using the AddDeserializer API to enable deserializing multiple different formats.
reviewed: 2016-08-09
component: Core
related:
 - nservicebus/serialization
 - samples/pipeline/multi-serializer
---


This sample uses the AddDeserializer API to illustrate a receiving endpoint deserializing multiple different serialization formats.



## Sending Endpoints

There are multiple sending endpoints, one per serializer.


### ExternalNewtonsoftJsonEndpoint

Send messages using the external [Json.NET Serializer](/nservicebus/serialization/newtonsoft.md) in Json format.

snippet: configExternalNewtonsoftJson


### ExternalNewtonsoftBsonEndpoint

Send messages using the external [Json.NET Serializer](/nservicebus/serialization/newtonsoft.md) in Bson format.

snippet: configExternalNewtonsoftBson


### JilEndpoint

Send messages using the [Jil Serializer](/samples/serializers/jil/).

snippet: configJil


### MessagePackEndpoint

Send messages using the [MessagePack Serializer](/samples/serializers/message-pack/).

snippet: configMessagePack


### WireEndpoint

Send messages using the [Wire Serializer](/samples/serializers/wire/).

snippet: configWire


### XmlEndpoint

Send messages using the [Xml Serializer](/nservicebus/serialization/xml.md).

snippet: configXml


## Shared

Contains message definitions and a [message mutator](/nservicebus/pipeline/message-mutators.md) that logs the outgoing data. Shared by all endpoints and configured by `endpointConfiguration.RegisterOutgoingMessageLogger();` in the above snippets.

snippet: outgoingmutator


## ReceivingEndpoint

The receiving endpoints references all the serializes used above and adds them using `AddDeserializer`

snippet: configAll

There is also a [message mutator](/nservicebus/pipeline/message-mutators.md) that logs the incoming data.

snippet: incomingmutator
