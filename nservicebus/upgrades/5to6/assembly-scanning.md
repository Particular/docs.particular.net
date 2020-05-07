---
title: Assembly Scanning Changes in NServiceBus Version 6
reviewed: 2020-05-07
component: Core
related:
 - nservicebus/hosting/assembly-scanning
 - transports/upgrades/sqlserver-2to3
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 5
 - 6
---

## Nested directories not scanned by default

The default behavior of version 6 is to not scan nested directories for assemblies. This behavior can be re-enabled using the [assembly scanning API](/nservicebus/hosting/assembly-scanning.md#nested-directories).


## Scanning uses an "exclude" approach

The API has been changed to an "exclude a list" approach. See [Assemblies to scan](/nservicebus/hosting/assembly-scanning.md#assemblies-to-scan) for more information.

snippet: 5to6ScanningUpgrade
