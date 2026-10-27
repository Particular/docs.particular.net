---
title: ServiceControl Storage
summary: Storage options for ServiceControl Error and Audit instances.
reviewed: 2026-10-27
component: ServiceControl
redirects:
  - servicecontrol/ravendb
---

ServiceControl Error and Audit instances store the messages they ingest so that ServicePulse can display and search them. Storage is selected per instance; a Monitoring instance does not need a database.

[RavenDB](/servicecontrol/storage/ravendb/) is the default for Error and Audit instances. It is embedded when deploying with PowerShell or the ServiceControl Management Utility and runs separately for container deployments.

## Storage providers

Choose a provider for each instance. Follow the linked configuration guide to prepare its storage and configure ServiceControl.

#if-version (,7)

| Storage | Availability | Configuration |
| --- | --- | --- |
| RavenDB | Default for Error and Audit instances | [Configure RavenDB storage](/servicecontrol/storage/ravendb/) |

#end-if

#if-version [7,)

Each Error and Audit instance uses one storage provider, chosen when the instance is created. Instances can use different providers; for example, an Error instance can use SQL Server while its Audit instances use RavenDB.

- **RavenDB** is the default. PowerShell and ServiceControl Management Utility deployments embed it, so no separate database server is needed. It suits teams that do not already run SQL Server or PostgreSQL.
- **SQL Server** and **PostgreSQL** suit teams that already run, monitor, and back up one of these database servers and want ServiceControl data managed with the same tools.

Before choosing SQL Server or PostgreSQL, check how they differ from RavenDB:

- Error instances also need an [external message body store](#external-message-body-storage).
- Audit instances store only text message bodies; binary bodies cannot be viewed in ServicePulse.
- SQL Server must have Full-Text Search installed.
- Existing RavenDB instances cannot yet be [migrated to SQL storage](/servicecontrol/migrations/ravendb-to-sql/).

| Provider | `PersistenceType` value | Requirements | Configuration |
| --- | --- | --- | --- |
| RavenDB (default) | `RavenDB` | None for embedded deployments; a RavenDB container for container deployments | [Configure RavenDB storage](/servicecontrol/storage/ravendb/) |
| SQL Server | `SQLServer` | An existing database on a server with Full-Text Search; an external message body store for Error instances | [Configure SQL Server storage](/servicecontrol/storage/sqlserver/) |
| PostgreSQL | `PostgreSQL` | An existing database; an external message body store for Error instances | [Configure PostgreSQL storage](/servicecontrol/storage/postgresql/) |

Configure SQL storage separately for each [Error instance](/servicecontrol/servicecontrol-instances/configuration.md#storage) and [Audit instance](/servicecontrol/audit-instances/configuration.md#storage).

## Sharing a database

Keep Error and Audit data apart, either in separate databases or in separate schemas of one database. Separation keeps each instance's permissions apart, and separate databases also let each instance's data be backed up, restored, and sized independently.

- **Separate databases:** each database can have its own storage and server. Audit data is usually much larger than Error data and is often less critical to recover, so this is the more flexible choice for production.
- **Separate schemas in one database:** simpler to provision, and permissions can still be scoped to each schema. SQL Server backs up and restores whole databases, not schemas, so restoring returns both instances' data to the same point in time. PostgreSQL `pg_dump` and `pg_restore` can select one schema with `-n`, but point-in-time recovery still restores the whole server.

An Error instance and an Audit instance can use the same SQL Server or PostgreSQL database, but each needs its own schema. The default settings already do this: the Error instance uses the connection's default schema, and the Audit instance uses `sc_audit`. Setup fails if an Error instance and an Audit instance are configured to use the same schema.

## External message body storage

SQL-backed Error instances also require an external message body store. Small text bodies are stored in the database; larger bodies and all binary bodies use the selected external store. See [what is stored where](/servicecontrol/storage/external-body/#inline-storage-and-compression). SQL-backed Audit instances do not require an external message body store.

| External body storage | Use when | Configuration |
| --- | --- | --- |
| File system | A durable directory or mounted volume is available to the Error instance | [Configure file system storage](/servicecontrol/storage/external-body/file.md) |
| Azure Blob Storage | Message bodies should be stored in an Azure Storage account | [Configure Azure Blob Storage](/servicecontrol/storage/external-body/azure-blob.md) |
| Amazon S3 | Message bodies should be stored in an S3 bucket | [Configure Amazon S3 storage](/servicecontrol/storage/external-body/amazon-s3.md) |

See the [external message body storage overview](/servicecontrol/storage/external-body/) for shared configuration and retention guidance.

## Backup and restore

Back up and monitor each instance's database and, for SQL-backed Error instances, its external message body store. Use the database and body-store providers' backup and availability guidance; the [RavenDB backup procedure](/servicecontrol/storage/ravendb/backup-sc-database.md) does not apply to SQL storage.

> [!WARNING]
> Back up and restore the database and the external message body store together. Restoring only one leaves messages whose bodies are missing, or bodies that no message refers to.

## Changing the storage provider

> [!WARNING]
> Changing `PersistenceType` on an existing instance does not move its data. The instance starts with empty storage. See [Migrating from RavenDB to SQL storage](/servicecontrol/migrations/ravendb-to-sql/).

#end-if
