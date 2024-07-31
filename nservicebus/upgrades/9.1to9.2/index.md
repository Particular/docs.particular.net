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

In NServiceBus 9.2.x, the DataBus feature was removed from `NServiceBus.Core` and released as it's own [NServiceBus.ClaimCheck.DataBus](https://www.nuget.org/packages/NServiceBus.Transport.Msmq/) package.

The namespace for the DataBus feature has  also changed from `NServiceBus.DataBus` to `NServiceBus.ClaimCheck.DataBus`.

Update endpoint configuration code to use the new package and namespace. The API surface between the old and the new code remains the same.