---
title: System.Text.Json Serializer
summary: A json serializer using System.Text.Json
reviewed: 2023-06-12
component: SystemJson
related:
 - samples/serializers/xml
---

## Usage

snippet: SystemJsonSerialization

## Caveats

include: interface-not-supported

### Unsupported types

* [System.Type](https://learn.microsoft.com/en-us/dotnet/api/system.type)
* Not all [collection types are supported](https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/supported-collection-types)
