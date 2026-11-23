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

The System.Text.Json serializer is more limited than Newtonsoft.Json. See [Migrate from Newtonsoft.Json to System.Text.Json](https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/migrate-from-newtonsoft) for more details.

If needed, both serializers can be used side-by-side during a transition period by using [multiple deserializers](/nservicebus/serialization/#specifying-additional-deserializers). For this to work, a custom content type needs to be specified as shown above.

## Migration from the community version

> [!NOTE]
> Thanks to [Simon Cropp](https://github.com/SimonCropp) who built [the community version of the serializer](https://github.com/simoncropparchived/NServiceBus.Json) and donated it to Particular Software.

The serializer is mostly compatible with the community version. See the [upgrade guide](/nservicebus/upgrades/community-system-json.md) for more details.
