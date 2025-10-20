---
title: Backup the ServiceControl Data
summary: How to backup the ServiceControl RavenDB instance
reviewed: 2025-02-14
---
ServiceControl uses RavenDB for data storage. To backup or restore the database instance, follow these procedures:

## Instances using RavenDB 5

> [!NOTE]
> Applies to ServiceControl 4.26.0 or newer instances using the RavenDB 5 engine

For ServiceControl instances using RavenDB 5, refer to the [official RavenDB documentation on how to perform a backup and restore data](https://ravendb.net/docs/article-page/5.4/csharp/server/ongoing-tasks/backup-overview).

## Instances using RavenDB 3.5

> [!NOTE]
> Applies to:
> * ServiceControl 4.26.0 or newer instances using the RavenDB 3.5 engine
> * ServiceControl 4.25.x or lower

### Backup

For ServiceControl instances using RavenDB 3.5, the following steps outline how to perform a data backup:

 1. Open ServiceControl Management to view the list of ServiceControl service instances
 1. Stop the service using the action icons
   - For extra safety consider temporarily Disabling the service in the Windows Services Management Console (Run `services.msc`)
 1. Click the link under data path to go to the data directory
 1. Copy or archive the contents of the data directory
 1. Start the service again once the copy is complete
   - If the services was temporarily disabled restore it Automatic in the Windows Services Management Console (Run `services.msc`)

### Restore

 1. Open ServiceControl Management to view the list of ServiceControl service instances
 1. Stop the service from the action icons
   - For extra safety consider temporarily Disabling the service in the Windows Services Management Console (Run `services.msc`)
 1. Click the link under data path to go to the data directory
 1. Replace the contents of this directory with the previously copied data
 1. Start the service again once the copy is complete
   - If the services was temporarily disabled restore it Automatic in the Windows Services Management Console (Run `services.msc`)

## Important notes and restrictions

### Do not migrate to an older Windows Server version

Care should be taken when planning to move ServiceControl from one server to another. Moving databases between servers can be problematic. RavenDB does not support moving from a new versions of Windows back to older versions of Windows. See [Getting error while restoring backup file in raven DB](https://stackoverflow.com/questions/25625910/getting-error-while-restoring-backup-file-in-raven-db) for more details.

### Restore to the same ServiceControl version

The ServiceControl database should not be restored to older copies of the ServiceControl service. This is not supported as both the database structure and the version on RavenDB may change between versions. These changes aren't always backward or forwards compatible.

### Restore using the same Windows Service name

When restoring a ServiceControl database, the name of the ServiceControl Windows Service must be the same on the target machine as it was on the source machine.
