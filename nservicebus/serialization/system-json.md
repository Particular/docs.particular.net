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

## Compatibility with Newtonsoft.Json

When migrating from Newtonsoft.Json, consult [Migrate from Newtonsoft.Json to System.Text.Json](https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/migrate-from-newtonsoft) for the differences in default behavior and supported features.

If needed, both serializers can be used side-by-side during a transition period by using [multiple deserializers](/nservicebus/serialization/#specifying-additional-deserializers). For this to work, a custom content type needs to be specified as shown above.
