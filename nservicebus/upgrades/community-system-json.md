---
title: Upgrading from community System.Text.Json serializer
summary: Instructions on how to migrate from the community System.Text.Json serializer to the supported version.
reviewed: 2023-06-12
component: SystemJson
related:
 - nservicebus/serialization
isUpgradeGuide: true
---

Message serialization with System.Text.Json is bundled into NServiceBus version 8.1 and above. The built-in serializer is fully compatible with the community-created [NServiceBus.Json](https://github.com/NServiceBusExtensions/NServiceBus.Json) package.

## Conversion from community package

First, upgrade to NServiceBus 8.1.0 or newer.

Then, instead of:

```csharp
var serialization = endpointConfiguration.UseSerialization<NServiceBus.Json.SystemJsonSerializer>();
```

Remove the reference to the [NServiceBus.Json NuGet package](https://www.nuget.org/packages/NServiceBus.Json) and use:

```csharp
var serialization = endpointConfiguration.UseSerialization<NServiceBus.SystemJsonSerializer>();
```

If full namespaces are not used, this code change may not be necessary, and instead, the `using NServiceBus.Json;` statement can be removed.

## API differences

### Specifying content type

The `ContentTypeKey(...)` method has been renamed to `ContentType(...)`.

### Options

The low level `.ReaderOptions(...)` and `WriterOptions(...)` is no longer available and it's recommended to use the [JsonSerializerOptions](https://learn.microsoft.com/en-us/dotnet/api/system.text.json.jsonserializeroptions) to [control serialization and deserialization behavior](/nservicebus/serialization/system-json.md#customizing-serialization-options).
