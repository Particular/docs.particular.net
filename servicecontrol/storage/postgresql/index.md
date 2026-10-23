---
title: ServiceControl PostgreSQL Storage
summary: Configure PostgreSQL storage for ServiceControl Error and Audit instances.
reviewed: 2026-06-01
component: ServiceControl
versions: '[7,)'
---

From ServiceControl 7, Error and Audit instances can use PostgreSQL instead of the default [RavenDB storage](/servicecontrol/storage/ravendb/). Select PostgreSQL when an instance should use an existing PostgreSQL deployment. Storage is configured separately for each instance.

> [!IMPORTANT]
> When using PostgreSQL, Error instances require [external message body storage](/servicecontrol/storage/external-body/) in addition to the database, even when message bodies fit inline. Audit instances do not require it.

## Recommended PostgreSQL version

PostgreSQL 18 or later is recommended for ServiceControl storage. PostgreSQL 18 supports [parallel GIN index builds](https://www.postgresql.org/docs/18/sql-createindex.html), which significantly improved ServiceControl performance in testing by Particular Software. Use the latest minor release for the selected PostgreSQL major version.

## Prepare the database

Create a database before configuring the instance. If `Database/Schema` is not set, ServiceControl uses the connection's default `search_path`, which is normally `public`. A configured schema must already exist and the instance's database user must have rights on it. ServiceControl does not create the database or schema.

PostgreSQL full-text search uses a built-in `tsvector` GIN index and message body search is always enabled for PostgreSQL storage, regardless of the [`EnableFullTextSearchOnBodies`](/servicecontrol/servicecontrol-instances/configuration.md#performance-tuning-servicecontrolenablefulltextsearchonbodies) setting.

Run the instance setup process (`--setup`) before normal operation. For Error instances, setup applies database migrations and provisions the external message body store. For Audit instances, database migrations run at every startup and daily partitions are provisioned automatically, so the runtime identity must have permissions to modify the schema and manage partitions (create and alter tables, create and drop partitions). See the [Error container setup](/servicecontrol/servicecontrol-instances/deployment/containers.md#initial-setup) or [Audit container setup](/servicecontrol/audit-instances/deployment/containers.md#initial-setup) instructions when deploying in containers.

The ServiceControl installer does not yet create SQL-backed Audit instances; configure their settings using environment variables or the application configuration file.

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

PostgreSQL-backed Audit instances are partitioned by day. The runtime provisions and drops daily partitions automatically.

Audit instances do not require an external body store; eligible text bodies are stored inline in the database.

include: sc-audit-maxbodysizetostore

`ServiceControl.Audit/MaxBodySizeToStore` is a hard cap for Audit instances. Message bodies larger than the configured limit, or bodies that are not text, are not available for viewing.

SQL-backed Audit instances do not support maintenance mode.

Connect to PostgreSQL directly or through a session-mode connection pooler. Transaction-mode poolers break the advisory lock that the retention process holds, which stops retention from running.

Example environment variables for an Audit instance:

```text
SERVICECONTROL_AUDIT_PERSISTENCETYPE=PostgreSQL
SERVICECONTROL_AUDIT_DATABASE_CONNECTIONSTRING="Host=postgres;Database=ServiceControl.Audit;Username=sc;Password=..."
SERVICECONTROL_AUDIT_AUDITRETENTIONPERIOD=30
```

See the [Audit instance settings reference](/servicecontrol/audit-instances/configuration.md#storage) for the complete reference.

## Message bodies and operations

See the [external body storage overview](/servicecontrol/storage/external-body/) for Error-instance inline size, compression, and body retention settings. Follow the [storage backup and restore guidance](/servicecontrol/storage/#backup-and-restore) for database and message body backups; RavenDB-specific procedures do not apply.
