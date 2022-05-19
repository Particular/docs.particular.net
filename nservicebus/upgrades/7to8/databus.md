---
title: Upgrading Databus from Version 7 to 8
summary: Instructions on how to upgrade databus usage from NServiceBus version 7 to version 8.
reviewed: 2022-05-19
component: Core
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 7
 - 8
---

The BinaryFormatter serializer that is used internally in NServiceBus version 7 is made obsolete, as it is being phased out by Microsoft, due to security concerns. To achieve this, the DataBus configuration API now accepts a serializer. The recommended serializer is `SystemJsonDataBusSerializer` that is built-in and uses `System.Text.Json` library. The usage of the API needs to be changed accordingly:

```csharp
endpointConfiguration.UseDataBus<FileShareDataBus, SystemJsonDataBusSerializer>();
```

The serializer (IDataBusSerializer) can still be registered via Dependency Injection, but it is recommended to move away from this and use the configuration API above. The dependency injection integration will be removed in the future version of NServiceBus.

## Custom Serializers

The `IDataBusSerializer` interface is changed to better isolate type information causing security concerns. Custom implementations of this interface should ignore type information embedded in the persisted payload and use the `propertyType` passed to the `Deserialize` method:

```csharp
public object Deserialize(Type propertyType, Stream stream)
```

## Fallback Behavior

The caught exceptions during deserialization of DataBus messages means that the fallback serializer (`BinaryFormatterDataBusSerializer`) will be tried. This behavior is enabled by default and will be removed in the future versions of NServiceBus. The header (NServiceBus.DataBus.Serializer) will specify which serializer was used for the DataBus property.
