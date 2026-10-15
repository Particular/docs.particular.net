---
title: ServiceControl SQL Server Storage
summary: Configure SQL Server storage for ServiceControl Error and Audit instances.
reviewed: 2026-06-01
component: ServiceControl
versions: '[7,)'
---

From ServiceControl 7, Error and Audit instances can use SQL Server instead of the default [RavenDB storage](/servicecontrol/storage/ravendb/). Select SQL Server when an instance should use an existing SQL Server deployment. Storage is configured separately for each instance.

## Prepare the database

Create a database and schema before configuring the instance. The default schema is `dbo`. ServiceControl does not create the database or schema; both must exist before setup runs. Schema names can contain letters, digits, and underscores, must start with a letter or underscore, and are limited to 63 characters.

SQL Server Full-Text Search must be installed for both Error and Audit instances. Error instance setup and Audit instance startup fail if the feature is not present on the server. Message body search is always enabled for SQL Server storage, regardless of the [`EnableFullTextSearchOnBodies`](/servicecontrol/servicecontrol-instances/configuration.md#performance-tuning-servicecontrolenablefulltextsearchonbodies) setting.

Run the instance setup process before normal operation. For Error instances, setup applies database migrations and provisions the external message body store. For Audit instances, database migrations run at every startup, so the runtime identity must have permissions to modify the schema (create and alter tables, indexes). See the [Error container setup](/servicecontrol/servicecontrol-instances/deployment/containers.md#initial-setup) or [Audit container setup](/servicecontrol/audit-instances/deployment/containers.md#initial-setup) instructions when deploying in containers.

The ServiceControl installer does not yet create SQL-backed Audit instances; configure their settings using environment variables or the application configuration file.

## Error instance configuration

1. Set `ServiceControl/PersistenceType` to `SQLServer` and `ServiceControl/Database/ConnectionString` to the database connection string. If using a custom schema, set `ServiceControl/Database/Schema`.
2. Configure an [external message body store](/servicecontrol/storage/external-body/). This is required even if most bodies fit inline in the database.
3. Set [`ServiceControl/ErrorRetentionPeriod`](/servicecontrol/servicecontrol-instances/configuration.md#data-retention-servicecontrolerrorretentionperiod) to between five and 45 days.

Set these variables in the Error instance's environment to use file system body storage. Replace the connection string placeholder with credentials supplied by the deployment environment. These storage settings supplement the instance's transport and other required settings.

```text
SERVICECONTROL_PERSISTENCETYPE=SQLServer
SERVICECONTROL_DATABASE_CONNECTIONSTRING="Server=sqlserver;Database=ServiceControl;User Id=sc;Password=..."
SERVICECONTROL_MESSAGEBODY_STORAGETYPE=FileSystem
SERVICECONTROL_MESSAGEBODY_FILESYSTEM_STORAGEPATH=/var/servicecontrol/bodies
SERVICECONTROL_ERRORRETENTIONPERIOD=15
```

See the [Error instance settings reference](/servicecontrol/servicecontrol-instances/configuration.md#storage) for the complete reference.

## Audit instance configuration

1. Set `ServiceControl.Audit/PersistenceType` to `SQLServer` and `ServiceControl.Audit/Database/ConnectionString` to the database connection string. If using a custom schema, set `ServiceControl.Audit/Database/Schema`.
2. Set [`ServiceControl.Audit/AuditRetentionPeriod`](/servicecontrol/audit-instances/configuration.md#data-retention-servicecontrol-auditauditretentionperiod).

Audit instances do not require an external body store; eligible text bodies are stored inline in the database. [`ServiceControl.Audit/MaxBodySizeToStore`](/servicecontrol/audit-instances/configuration.md#performance-tuning-servicecontrol-auditmaxbodysizetostore) is a hard cap for Audit instances. Message bodies larger than the configured limit, or bodies that are not text, are not stored at all.

SQL-backed Audit instances do not support maintenance mode.

Example environment variables for an Audit instance:

```text
SERVICECONTROL_AUDIT_PERSISTENCETYPE=SQLServer
SERVICECONTROL_AUDIT_DATABASE_CONNECTIONSTRING="Server=sqlserver;Database=ServiceControl.Audit;User Id=sc;Password=..."
SERVICECONTROL_AUDIT_AUDITRETENTIONPERIOD=30
```

See the [Audit instance settings reference](/servicecontrol/audit-instances/configuration.md#storage) for the complete reference.

## Message bodies and operations

See the [external body storage overview](/servicecontrol/storage/external-body/) for Error-instance inline size, compression, and body retention settings. Follow the [storage backup and restore guidance](/servicecontrol/storage/#backup-and-restore) for database and message body backups; RavenDB-specific procedures do not apply.
