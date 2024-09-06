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


The NServiceBus.ClaimCheck library is line-level compatible with NServiceBus.DataBus, endpoints using NServiceBus.DataBus need not be upgraded to ClaimCheck all at once .


