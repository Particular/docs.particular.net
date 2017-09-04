---
title: NServiceBus Host Upgrade Version 7 to 8
summary: Instructions on how to upgrade NServiceBus Host Version 7 to 8.
reviewed: 2016-04-06
component: Host
related:
 - nservicebus/upgrades/6to7
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 6
 - 7
---

The NServiceBus will be deprecated as of Version 9 and users are recommended to avoid using it for new endpoints. Upgrading existing endpoints is still supported for Version 8

## Incompatible with NuGet X.Y

NuGet no longer support packages adding files and modifying project files. This means that installing the host won't result in a runnable endpoint.

TBD: Is it possible/should we add instructions on how to manually modify the csproj to get this working?


## Incompatible with the new Visual Studio project system

TBD: What is actally not compatible?