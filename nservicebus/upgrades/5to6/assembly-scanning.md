---
title: Assembly scanning changes in Version 6
reviewed: 2016-10-26
tags:
 - upgrade
 - migration
related:
 - nservicebus/hosting/assembly-scanning
 - nservicebus/upgrades/sqlserver-2to3
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 5
 - 6
---

## Nested Directories

The default behavior of Version 6 is to not scan nested directories for assemblies. This behavior can re-enable using the [Assembly Scanning API](/nservicebus/hosting/assembly-scanning.md#nested-directories).


## Include moved to Exclude

The API has been changed to an "Exclude a list" approach. See [Assemblies to scan](/nservicebus/hosting/assembly-scanning.md#assemblies-to-scan) for more information.

snippet:5to6ScanningUpgrade