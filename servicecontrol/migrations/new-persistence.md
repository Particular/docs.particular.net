---
title: New persistence format
summary: How to migrate ServiceControl Audit instances to the new persistence format introduced in version 4.26
isUpgradeGuide: true
reviewed: 2025-02-10
component: ServiceControl
redirects:
  - servicecontrol/upgrades/new-persistence
---

Version 4.26 of ServiceControl introduced a new persistence format for audit instances. The new persistence format is faster and more efficient, using less space on disk.

New audit instances created with ServiceControl version 4.26 and above will use the new persistence format. Existing audit instances created with ServiceControl version 4.25 and below will continue to use the old persistence format, even if it is upgraded.

> [!WARNING]
> Updating an existing audit instance to version 4.26 and above will _not_ automatically change the persistence format.

It is recommended to update to the new persistence format. The old persistence format will remain supported for a limited time to enable this transition.

## How to switch to the new persistence format

Changing the persistence format of an existing audit instance is not supported.

To switch to the new persistence format, follow the steps for [zero downtime upgrades](replacing-audit-instances/), ensuring that the new audit instance is created with ServiceControl 4.26 or above.

## How to determine which persistence format is used

Audit instances created using ServiceControl version 4.25 and below use the old persistence format. Even if this instance is upgraded to a newer version, it will continue to use the old format.

The persistence format of an instance can be verified using ServiceControl Management or the `Get-ServiceControlAuditInstances` powershell cmdlet after installing ServiceControl 4.26 or above.

If the value is `RavenDB 5` (ServiceControl 4.x) or `RavenDB` (ServiceControl 5.x or later) then the instance is using the new persistence format and does not require upgrading.

If the value is `RavenDB 3.5` then the instance is using the old persistence format and should follow the [instructions above](#how-to-switch-to-the-new-persistence-format) to migrate to the new persistence format.
