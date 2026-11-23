---
title: System.Text.Json serializer
summary: Serialize NServiceBus messages with System.Text.Json, the recommended JSON serializer for new projects, and configure its content type and options.
reviewed: 2026-11-23
component: SystemJson
related:
 - samples/serializers/system-json
---

The System.Text.Json message serializer uses the built-in JSON serialization in .NET to serialize and deserialize messages. This serializer should be the default choice for serialization in new projects.

## Usage

snippet: SystemJsonSerialization

### Specifying content type

The default content type is `application/json`. It can be changed using:

snippet: SystemJsonContentType

> [!WARNING]
> Adding a suffix like `; systemjson` requires **all** endpoints involved to use this case-sensitive full key. See [NServiceBus.ContentType documentation](/nservicebus/messaging/headers.md#serialization-headers-nservicebus-contenttype) for more information.

### Customizing serialization options

To control how serialization and deserialization are performed, pass in [JsonSerializerOptions](https://learn.microsoft.com/en-us/dotnet/api/system.text.json.jsonserializeroptions):

snippet: SystemJsonOptions

## Trimming and Native AOT

Registering message types with a source-generated `JsonSerializerContext` as the `TypeInfoResolver` of the [serializer options](#usage-customizing-serialization-options) avoids reflection-based metadata and makes the serializer trimming and Native AOT friendly. See [reflection vs source generation](https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/reflection-vs-source-generation) for how source generation works in System.Text.Json.
