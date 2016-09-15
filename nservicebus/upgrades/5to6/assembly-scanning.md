---
title: Assembly scanning changes in Version 6
tags:
 - upgrade
 - migration
related:
- nservicebus/hosting/assembly-scanning
- nservicebus/upgrades/sqlserver-2to3
---

## Nested Directories

NServiceBus Version 6 is no longer scanning nested directories for assemblies. This behavior can re-enable using the [Assembly Scanning API](/nservicebus/hosting/assembly-scanning.md#nested-directories).


## Include moved to Exclude

In Version 6 the API has been changed to an "Exclude a list" approach. See [Assemblies to scan](/nservicebus/hosting/assembly-scanning.md#assemblies-to-scan) for more information.

snippet:5to6ScanningUpgrade