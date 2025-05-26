---
title: System.Text.Json serializer
summary: A json serializer using System.Text.Json
reviewed: 2025-05-26
component: SystemJson
related:
 - samples/serializers/system-json
---

The System.Text.Json message serializer uses the built-in JSON serialization in .NET to serialize and deserialize messages. This serializer should be the default choice for serialization in new projects.

## Usage

snippet: SystemJsonSerialization

### Specifying content type

The default content type used is `application/json`, but can be changed using:

snippet: SystemJsonContentType

> [!WARNING]
> Adding a suffix like `; systemjson` requires **all** endpoint involved to use this case-sensitive full key. See [NServiceBus.ContentType documentation](/nservicebus/messaging/headers.md#serialization-headers-nservicebus-contenttype) for more information

### Customizing serialization options

To control how the serialization and deserialzation is performed [JsonSerializerOptions](https://learn.microsoft.com/en-us/dotnet/api/system.text.json.jsonserializeroptions) can be passed in:

snippet: SystemJsonOptions

## Compatibility with Newtonsoft.Json

The System.Text.Json serializer is more limited compared to Newtonsoft.Json, see the [upgrade guide](https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/migrate-from-newtonsoft) for more details.

If needed, both serializers can be used side-by-side during a transition period by using [multiple deserializers](/nservicebus/serialization/#specifying-additional-deserializers). For this to work, a custom content type needs to be specified as shown above.

## Migration from the community version

> [!NOTE]
> Thanks to [Simon Cropp](https://github.com/SimonCropp) who built [the community version of the serializer](https://github.com/NServiceBusExtensions/NServiceBus.Json) and donated it to Particular Software.

The serializer is mostly compatible with the community version, see the [upgrade guide](/nservicebus/upgrades/community-system-json.md) for more details.
