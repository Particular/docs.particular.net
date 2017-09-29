---
title: Utf8Json Serializer
summary: A JSON serializer that uses Utf8Json.
component: Utf8Json
reviewed: 2017-09-29
related:
 - samples/serializers/utf8json
---

Using [JSON](https://en.wikipedia.org/wiki/Json) via a NuGet dependency on [Utf8Json](https://github.com/neuecc/Utf8Json).


## Usage

snippet: Utf8JsonSerialization


### Resolver

Customizes the instance of [IJsonFormatterResolver](https://github.com/neuecc/Utf8Json#resolver) used for serialization.

snippet: Utf8JsonResolver


include: custom-contenttype-key

snippet: Utf8JsonContentTypeKey


## Currently not supported

Usages of `DataBusProperty<T>` since it doesn't have a default constructor. However usage of the [databus convention](/nservicebus/messaging/databus) is supported.
