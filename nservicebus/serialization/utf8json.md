---
title: Utf8Json Serializer
summary: A JSON serializer that uses Utf8Json.
component: Utf8Json
reviewed: 2019-07-01
related:
 - samples/serializers/utf8json
---

Using [JSON](https://en.wikipedia.org/wiki/Json) via a NuGet dependency on [Utf8Json](https://github.com/neuecc/Utf8Json).

## Usage

snippet: Utf8JsonSerialization

### Resolver

It is possible to customize the instance of [IJsonFormatterResolver](https://github.com/neuecc/Utf8Json#resolver) used for serialization.

snippet: Utf8JsonResolver

include: custom-contenttype-key

snippet: Utf8JsonContentTypeKey

## Currently not supported

The use of `DataBusProperty<T>` is not supported because the property doesn't provide a default constructor. However, the use of the [databus conventions](/nservicebus/messaging/databus) is supported.
