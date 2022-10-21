---
title: New persistence format
summary: How to migrate ServiceControl Audit instances to the new persistence format introduced in version 4.26
isUpgradeGuide: true
reviewed: 2022-10-21
---

Version 4.26 of ServiceControl introduced a new persistence format for audit instances. The new persistence format is faster and more efficient, using less space on disk.

Any new audit instance created with ServiceControl version 4.26 and above will use the new persistence format. Any existing audit instance that was created with ServiceControl version 4.25 and below will continue to use the old persistence format, even if it is upgraded.

WARN: Updating an existing audit instance to version 4.26 and above will _not_ automatically change the persistence format.

It is recommended to update to the new persistence format. The old persistence format will remain supported for a limited time to enable this transition.

## How to switch to the new persistence format

Changing the persistence format of an existing audit instance is not supported.

To switch to the new persistence format, follow the steps for [zero downtime upgrades](zero-downtime.md), ensuring that the new audit instance is created with ServiceControl 4.26 or above.

## How to determine which persistence format is used

Any audit instance that was created using ServiceControl version 4.25 and below is using the old instance format. Even if this instance is upgraded to a newer version, it will continue to use the old format.

The persistence format of an instance can be verified like this:

1. Find the installation folder using ServiceControl Management or the `Get-ServiceControlAuditInstances` powershell cmdlet.
1. In the installation folder, open the file `ServiceControl.Audit.exe.config` in a text editor.
1. Look for an app setting with the key `ServiceControl.Audit/PersistenceType`.

If the value is `ServiceControl.Audit.Persistence.RavenDb.RavenDbPersistenceConfiguration, ServiceControl.Audit.Persistence.RavenDB5` (ends in 5) then the instance is using the new persistence format.

If the app setting is missing, or has a value like `ServiceControl.Audit.Persistence.RavenDb.RavenDbPersistenceConfiguration, ServiceControl.Audit.Persistence.RavenDB` (does not end in 5) then the instance is using the old persistence format.
