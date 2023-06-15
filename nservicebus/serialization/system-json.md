---
title: System.Text.Json Serializer
summary: A json serializer using System.Text.Json
reviewed: 2023-06-12
component: SystemJson
related:
 - samples/serializers/newtonsoft
---

## Usage

snippet: SystemJsonSerialization

Note: Thanks to [Simon Cropp](https://github.com/SimonCropp) who built [the community version of the serializer](https://github.com/NServiceBusExtensions/NServiceBus.Json) and donated it to Particular Software.

### Specifing content type

The default content type used is `application/json` but can be changed using:

snippet: SystemJsonContentType

## Compatibility with Newtonsoft.Json

The System.Text.Json serializer is more limited compared to Newtonsoft.Json, see the [upgrade guide](https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/migrate-from-newtonsoft) for more details.

## Migration from the community version

The serializer is mostly compatible with the community version, see the [upgrade guide](/nservicebus/upgrades/community-system-json.md) for more details.
