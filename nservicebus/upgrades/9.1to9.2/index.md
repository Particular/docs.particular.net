---
title: Upgrade Version 9.1 to 9.2
summary: Instructions on how to upgrade NServiceBus from version 9.1 to version 9.2.
reviewed: 2024-08-01
component: Core
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 9
---

## DataBus change

In NServiceBus 9.2.x, the DataBus feature was removed from `NServiceBus.Core` and released as it's own [NServiceBus.ClaimCheck](https://www.nuget.org/packages/NServiceBus.ClaimCheck/) package.

The namespace for the DataBus feature has also changed from `NServiceBus.DataBus` to `NServiceBus.ClaimCheck`.

Update endpoint configuration code to use the new package and namespace. The API surface between the old and the new code has changed to use the term ClaimCheck instead of DataBus.  The table below shows the mapping from the DataBus configuration types to their ClaimCheck equivalents.

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