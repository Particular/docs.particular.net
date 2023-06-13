---
title: Upgrading from community System.Text.Json serialiser
summary: Instructions on how to migrate from the community System.Text.Json serializer to the supported version.
reviewed: 2023-06-12
component: SystemJson
related:
 - nservicebus/serialization
isUpgradeGuide: true
---

## Community serializer now bundled into NServiceBus Core 8.1

The built-in serializer is fully compatible with the community version.

Instead of:

```csharp
var serialization = endpointConfiguration.UseSerialization<NServiceBus.Json.SystemJsonSerializer>();
```
Remove the reference to https://www.nuget.org/packages/NServiceBus.Json and use:

```csharp
var serialization = endpointConfiguration.UseSerialization<NServiceBus.SystemJsonSerializer>();
```

