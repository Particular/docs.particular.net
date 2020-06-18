---
title: Backup the ServiceControl Data
summary: How to backup the ServiceControl RavenDB Embedded instance
reviewed: 2020-06-18
---
ServiceControl uses an embedded RavenDB for data storage. To backup or restore the database instance, follow these procedures:

### Backup

 1. Open ServiceControl Management to view the list of ServiceControl service instances
  ![ServiceControl Management Utility](managementutil-instance.png 'width=500')
 1. Stop the service using the action icons
 1. Click the link under data path to go to the data directory
 1. Copy or archive the contents of the data directory
 1. Start the service again once the copy is complete


### Restore

 1. Open ServiceControl Management to view the list of ServiceControl service instances
 1. Stop the service from the action icons
 1. Click the link under data path to go to the data directory
 1. Replace the contents of this directory with the previously copied data
 1. Start the service again once the copy is complete


### Important notes and restrictions

Care should be taken when planning to move ServiceControl from one server to another. Moving databases between servers can be problematic. The embedded RavenDB does not support moving from a new versions of Windows back to older versions of Windows. See [Getting error while restoring backup file in raven DB](https://stackoverflow.com/questions/25625910/getting-error-while-restoring-backup-file-in-raven-db) for more details.

The ServiceControl database should not be restored to older copies of the ServiceControl service. This is not supported as both the database structure and the version on RavenDB may change between versions. These changes aren't always backward compatible.

When restoring a ServiceControl database, the name of the ServiceControl Windows Service must be the same on the target machine as it was on the source machine.
