---
title: Back up and restore ServiceControl data
summary: How to back up and restore the RavenDB databases used by ServiceControl Error and Audit instances
reviewed: 2026-11-24
redirects:
  - servicecontrol/backup-sc-database
---
ServiceControl Error and Audit instances store their data in RavenDB. Back up and restore that data with the RavenDB backup and restore features in RavenDB Studio. For backup types, schedules, and destinations, see the [RavenDB backup documentation](https://docs.ravendb.net/6.2/server/ongoing-tasks/backup-overview).

Back up every database that an instance uses. RavenDB Studio lists them. The default database names are:

| Instance | Databases |
| --- | --- |
| Error instance | `primary` and `throughput` |
| Audit instance | `audit` |

## Open RavenDB Studio

The RavenDB Studio URL depends on where the database runs.

### Embedded database

Instances installed with ServiceControl Management or PowerShell run an embedded database. Open RavenDB Studio on the server where the instance is installed:

* Error instance: `http://localhost:33334`, or the configured [database maintenance port](/servicecontrol/servicecontrol-instances/configuration.md#storage-servicecontroldatabasemaintenanceport)
* Audit instance: `http://localhost:44445`, or the configured [database maintenance port](/servicecontrol/audit-instances/configuration.md#host-settings-servicecontrol-auditdatabasemaintenanceport)

The database accepts connections from the local server only. It runs only while the instance runs, either normally or in [maintenance mode](/servicecontrol/storage/ravendb/accessing-database.md#windows-deployment-maintenance-mode). ServiceControl Management shows a link to RavenDB Studio in Advanced Options while the instance is in maintenance mode. The instance also writes the URL to its log at startup, as `Database Url`.

### External database

When the instance uses the `particular/servicecontrol-ravendb` container or an independently hosted RavenDB server, open RavenDB Studio at the URL of that server, which is the URL in the instance's connection string. The database container serves RavenDB Studio on port `8080`.

## Back up

Back up while the instance is running. A backup doesn't require stopping the instance or using maintenance mode.

In RavenDB Studio, [create a backup task](https://docs.ravendb.net/6.2/studio/database/tasks/backup-task) for each database. Use the **Backup** type, and set a schedule and a destination folder.

The RavenDB server writes the backup files, so the account that runs the database needs write access to the destination folder. For an embedded database, this is the account that runs the ServiceControl Windows service. Scheduled backups of an embedded database run only while the instance runs.

## Restore

Do not restore into a running instance. RavenDB restores a backup only into a new database, so the existing database must be deleted first. The instance must not ingest messages or serve API requests until the restore completes.

> [!WARNING]
> A restore replaces all data in the database. Data stored after the backup was taken is lost. Before deleting a database, confirm that the backup files are complete and that the RavenDB server can read them.

The RavenDB server reads the backup files from its own file system, and the restore requires a local path. For an embedded database, the account that runs the ServiceControl Windows service needs read access to the backup folder. For the database container, mount the backup folder into the container.

### Embedded database

1. In ServiceControl Management, click the wrench <kbd> :wrench: </kbd> icon of the instance to open Advanced Options.
1. Click **Start Maintenance Mode**. The instance restarts and runs only the database. It doesn't ingest messages, and the API is unavailable.
1. Click the **RavenDB Management Studio** link.
1. Delete each database to restore.
1. [Restore each database from its backup](https://docs.ravendb.net/6.2/studio/database/create-new-database/from-backup), using the same database name.
1. Wait until the restore completes.
1. Click **Stop Maintenance Mode**. This stops the instance.
1. Start the instance.

Without ServiceControl Management, set the `MaintenanceMode` setting of the [Error instance](/servicecontrol/servicecontrol-instances/configuration.md#storage-servicecontrolmaintenancemode) or [Audit instance](/servicecontrol/audit-instances/configuration.md#host-settings-servicecontrol-auditmaintenancemode) to `true` and restart the Windows service. When the restore completes, remove the setting or set it to `false`, then restart the Windows service.

After a restore, RavenDB rebuilds the indexes. Until indexing completes, ServicePulse might show incomplete data.

### External database

1. Stop the ServiceControl instance, for example by stopping the instance container. Leave the RavenDB server running. Do not use maintenance mode; from ServiceControl 7, an instance using an external database doesn't start in maintenance mode.
1. In RavenDB Studio, delete each database to restore.
1. [Restore each database from its backup](https://docs.ravendb.net/6.2/studio/database/create-new-database/from-backup), using the same database name.
1. Wait until the restore completes, then start the ServiceControl instance.

## Important notes and restrictions

### Do not migrate to an older Windows Server version

Care should be taken when planning to move ServiceControl from one server to another. Moving databases between servers can be problematic. RavenDB does not support moving from new versions of Windows back to older versions of Windows. See [Getting error while restoring backup file in raven DB](https://stackoverflow.com/questions/25625910/getting-error-while-restoring-backup-file-in-raven-db) for more details.

### Restore to the same ServiceControl version

The ServiceControl database should not be restored to older copies of the ServiceControl service. This is not supported as both the database structure and the version on RavenDB may change between versions. These changes aren't always backward or forwards compatible.

### Restore using the same Windows Service name

When restoring a ServiceControl database, the name of the ServiceControl Windows Service must be the same on the target machine as it was on the source machine.
