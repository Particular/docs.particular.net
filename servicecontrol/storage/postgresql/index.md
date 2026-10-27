---
title: ServiceControl PostgreSQL Storage
summary: Configure PostgreSQL storage for ServiceControl Error and Audit instances.
reviewed: 2026-10-27
component: ServiceControl
versions: '[7,)'
---

From ServiceControl 7, Error and Audit instances can use PostgreSQL instead of the default [RavenDB storage](/servicecontrol/storage/ravendb/). Select PostgreSQL when an instance should use an existing PostgreSQL deployment. Storage is configured separately for each instance.

> [!IMPORTANT]
> When using PostgreSQL, Error instances require [external message body storage](/servicecontrol/storage/external-body/) in addition to the database, even when message bodies fit inline. Audit instances do not require it.

## Recommended PostgreSQL version

PostgreSQL 18 or later is recommended for ServiceControl storage. PostgreSQL 18 supports [parallel GIN index builds](https://www.postgresql.org/docs/18/sql-createindex.html), which significantly improved ServiceControl performance in testing by Particular Software. Use the latest minor release for the selected PostgreSQL major version.

## Prepare the database

Create a database before configuring the instance as ServiceControl does not create the database. Setup creates the instance's schema if it does not exist. If the identity that runs setup cannot create schemas, create the schema in advance and grant that identity rights on it. Error and Audit instances can use the same database but not the same schema; see [sharing a database](/servicecontrol/storage/#sharing-a-database).

PostgreSQL full-text search uses a built-in `tsvector` GIN index and message body search is always enabled for PostgreSQL storage, regardless of the [`EnableFullTextSearchOnBodies`](/servicecontrol/servicecontrol-instances/configuration.md#performance-tuning-servicecontrolenablefulltextsearchonbodies) setting.

> [!IMPORTANT]
> Run the instance setup process (`--setup`) before first use and after every upgrade. Setup applies the database migrations; the instance does not apply them at startup.

Setup applies database migrations for both Error and Audit instances, so the identity that runs setup must have permissions to modify the schema (create and alter tables and indexes), and to create the schema if it does not exist yet (`CREATE` on the database). For Error instances, setup also provisions the external message body store. See the [Error container setup](/servicecontrol/servicecontrol-instances/deployment/containers.md#initial-setup) or [Audit container setup](/servicecontrol/audit-instances/deployment/containers.md#initial-setup) instructions when deploying in containers.

> [!WARNING]
> While running, Audit instances create daily partitions up to 48 hours ahead and drop expired ones. PostgreSQL only lets the owner of a partitioned table create partitions of it. Setup creates the Audit tables, so they are owned by the role that runs setup. Run setup and normal operation as the same role, or grant the setup role to the runtime role so that the runtime role counts as the owner. Otherwise the **Audit partition provisioning** custom check fails, and Audit ingestion stops when the provisioned partitions run out, within about two days of the last successful provisioning.

The ServiceControl Management Utility (SCMU) and the PowerShell module can create PostgreSQL-backed Error and Audit instances:

- **SCMU:** choose `PostgreSQL` as the **STORAGE TYPE** and enter the **DATABASE CONNECTION STRING**. For an Error instance, also choose the **MESSAGE BODY STORAGE** and fill in its settings.
- **PowerShell:** pass `-PersistenceType PostgreSQL` and `-DatabaseConnectionString` to `New-ServiceControlInstance` or `New-ServiceControlAuditInstance`, and leave out `-DBPath` and `-DatabaseMaintenancePort`, which apply only to RavenDB. For an Error instance, `-MessageBodyStorageType` selects `FileSystem` (the default), `AzureBlob`, or `S3`. See the [Error instance](/servicecontrol/servicecontrol-instances/deployment/powershell.md) and [Audit instance](/servicecontrol/audit-instances/deployment/powershell.md) PowerShell guides.

Neither tool sets `Database/Schema`, so instances they create use the default schema.

## Error instance configuration

Set the persistence type to `PostgreSQL`, provide the database connection string, and configure the retention period. Also configure an [external message body store](/servicecontrol/storage/external-body/), which is required even if most bodies fit inline in the database.

include: sc-error-persistencetype

include: sc-error-database-connectionstring

include: sc-error-database-schema

include: sc-error-errorretentionperiod

Set these variables in the Error instance's environment to use file system body storage. Replace the connection string placeholder with credentials supplied by the deployment environment. These storage settings supplement the instance's transport and other required settings.

```text
SERVICECONTROL_PERSISTENCETYPE=PostgreSQL
SERVICECONTROL_DATABASE_CONNECTIONSTRING="Host=postgres;Database=ServiceControl;Username=sc;Password=..."
SERVICECONTROL_MESSAGEBODY_STORAGETYPE=FileSystem
SERVICECONTROL_MESSAGEBODY_FILESYSTEM_STORAGEPATH=/var/servicecontrol/bodies
SERVICECONTROL_ERRORRETENTIONPERIOD=15
```

See the [Error instance settings reference](/servicecontrol/servicecontrol-instances/configuration.md#storage) for the complete reference.

## Audit instance configuration

Set the persistence type to `PostgreSQL`, provide the database connection string, and configure the retention period.

include: sc-audit-persistencetype

include: sc-audit-database-connectionstring

include: sc-audit-database-schema

include: sc-audit-auditretentionperiod

PostgreSQL-backed Audit instances are partitioned by day. The instance creates and drops daily partitions automatically, which requires the runtime role to own the Audit tables; see [Prepare the database](#prepare-the-database).

Audit instances do not use an external message body store; they store text message bodies in the database.

include: sc-audit-maxbodysizetostore

> [!NOTE]
> Audit instances using SQL storage store only text message bodies, up to `ServiceControl.Audit/MaxBodySizeToStore`. Binary bodies and larger bodies cannot be viewed in ServicePulse.

> [!NOTE]
> SQL Server and PostgreSQL storage do not support maintenance mode. An instance with `MaintenanceMode` set to `true` does not start.

> [!WARNING]
> Connect to PostgreSQL directly or through a connection pooler in session mode. A pooler in transaction mode, such as PgBouncer with `pool_mode = transaction`, breaks the advisory lock that the retention process holds, and retention stops running.

Example environment variables for an Audit instance:

```text
SERVICECONTROL_AUDIT_PERSISTENCETYPE=PostgreSQL
SERVICECONTROL_AUDIT_DATABASE_CONNECTIONSTRING="Host=postgres;Database=ServiceControl.Audit;Username=sc;Password=..."
SERVICECONTROL_AUDIT_AUDITRETENTIONPERIOD=30
```

See the [Audit instance settings reference](/servicecontrol/audit-instances/configuration.md#storage) for the complete reference.

## Message bodies and operations

See the [external body storage overview](/servicecontrol/storage/external-body/) for Error-instance inline size, compression, and body retention settings. Follow the [storage backup and restore guidance](/servicecontrol/storage/#backup-and-restore) for database and message body backups; RavenDB-specific procedures do not apply.
