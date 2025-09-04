---
title: Upgrade Version 9 to 10
summary: Instructions on how to upgrade NServiceBus from version 9 to version 10.
reviewed: 2025-09-03
component: Core
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 9
 - 10
---

include: upgrade-major

## .NET target framework

NServiceBus 10 targets .NET 10 only. Go to [supported frameworks and platforms](/nservicebus/upgrades/supported-platforms.md) to learn more about the support timeline.

## Nullability

Progress has been made to enable nullability accross the API surface.

### Behavioral changes

#### ContextBag

The ContextBag no longer allows storing `null` as a value. In order to store `null` it is required to decorate the value with wrapper class. So instead of

```csharp
context.Set<string>("YourKey", null);
```

use

```csharp
context.Set<string>("YourKey", new YourKeyValue());

public record YourKeyValue(string? someValueThatMightBeNull = null);
```

### API changes

This is section is relevant for projects that have nullability enabled.

The APIs below have been modified to accomodate to receive nullable parameters.

| API |
|---|
|`sendOptions.StartNewConversation`|
|``|


## NServiceBus.DataBus is now a separated package called NServiceBus.ClaimCheck

The DataBus feature has been removed from `NServiceBus.Core` and released as it's own package, called [NServiceBus.ClaimCheck](https://www.nuget.org/packages/NServiceBus.ClaimCheck/).
The namespace package has been updated from `NServiceBus.DataBus` to `NServiceBus.ClaimCheck` to reflect the new name.
The API has also been updated to use the term ClaimCheck instead of DataBus. 

Update endpoint configuration to use the new package and namespace.

The table below shows the mapping from the DataBus configuration types to their ClaimCheck equivalents.

| NServiceBus.DataBus  | NServiceBus.ClaimCheck |
| --- | --- |
| `endpointConfiguration.UseDataBus` | `endpointConfiguration.UseClaimCheck` |
| `NServiceBus.FileShareDataBus` | `NServiceBus.FileShareClaimCheck` |
| `NServiceBus.SystemJsonDataBusSerializer` | `NServiceBus.SystemJsonClaimCheckSerializer` |
| `NServiceBus.DataBusProperty<T>` | `NServiceBus.ClaimCheckProperty<T>` |

### Migrating message contracts

The NServiceBus.ClaimCheck library is line-level compatible with NServiceBus.DataBus, meaning, in-flight messages that are sent using NServiceBus.DataBus will be properly handled by endpoints that have been upgraded to use NServiceBus.ClaimCheck; this is also true in reverse.

Some care should be taken when migrating message contracts from `DataBusProperty<T>` to `ClaimCheckProperty<T>`.  While NServiceBus.DataBus and NServiceBus.ClaimCheck are line-level compatible, they are not runtime compatible.  An endpoint that is currently running NServiceBus.DataBus will not write properties that are `ClaimCheckProperty<T>` to the DataBus. The reverse is true of NServiceBus.ClaimCheck endpoints and `DataBusProperty<T>`.  To facilitate the migration, each endpoint will need a copy of the message contract that uses the supported property type.

Changing from using `DataBusProperty<T>` to specifying conventions for the claim check properties will be the easiest way to migrate whilst maintaining runtime compatibility between the new and old versions. If this is not possible, the message contracts can be versioned separately too.

If message contracts are in a versioned library that has been migrated to `ClamCheckProperty<T>`, then NServiceBus.DataBus endpoints can remain on an older version of the contracts library until they can be upgraded to NServiceBus.ClaimCheck.

If message contracts are not in a versioned library, a local copy of the messages can be made to facilitate the transition. In this case it is imperative that all class names, namespaces, and property names are exactly the same to make sure the message can be properly deserialized when it is received.
