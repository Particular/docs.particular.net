---
title: Backup the ServiceControl Data
summary: How to backup the ServiceControl RavenDB instance
reviewed: 2026-09-18
redirects:
  - servicecontrol/backup-sc-database
---
To back up or restore the RavenDB database used by ServiceControl, refer to the [official RavenDB documentation on how to perform a backup and restore data](https://ravendb.net/docs/article-page/5.4/csharp/server/ongoing-tasks/backup-overview).

## Important notes and restrictions

### Do not migrate to an older Windows Server version

Care should be taken when planning to move ServiceControl from one server to another. Moving databases between servers can be problematic. RavenDB does not support moving from new versions of Windows back to older versions of Windows. See [Getting error while restoring backup file in raven DB](https://stackoverflow.com/questions/25625910/getting-error-while-restoring-backup-file-in-raven-db) for more details.

### Restore to the same ServiceControl version

The ServiceControl database should not be restored to older copies of the ServiceControl service. This is not supported as both the database structure and the version on RavenDB may change between versions. These changes aren't always backward or forwards compatible.

### Restore using the same Windows Service name

When restoring a ServiceControl database, the name of the ServiceControl Windows Service must be the same on the target machine as it was on the source machine.
