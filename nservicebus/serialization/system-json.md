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

## Compatibility with Newtonsoft.Json

The System.Text.Json serializer is more limited compared to Newtonsoft.Json, see the [upgrade guide](https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/migrate-from-newtonsoft) for more details.

## Unsupported types

* [System.Type](https://learn.microsoft.com/en-us/dotnet/api/system.type)
* Not all [collection types are supported](https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/supported-collection-types)
