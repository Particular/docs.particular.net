---
title: ServiceControl Storage
summary: Storage options for ServiceControl Error and Audit instances.
reviewed: 2026-06-01
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

| Storage | 🍅Availability | Configuration |
| --- | --- | --- |
| RavenDB | Default for Error and Audit instances | [Configure RavenDB storage](/servicecontrol/storage/ravendb/) |
| SQL Server | Error and Audit instances from ServiceControl 7; requires an existing database and schema | [Configure SQL Server storage](/servicecontrol/storage/sqlserver/) |
| PostgreSQL | Error and Audit instances from ServiceControl 7; requires an existing database and schema | [Configure PostgreSQL storage](/servicecontrol/storage/postgresql/) |

Configure SQL storage separately for each [Error instance](/servicecontrol/servicecontrol-instances/configuration.md#storage) and [Audit instance](/servicecontrol/audit-instances/configuration.md#storage).

## External message body storage

SQL-backed Error instances also require an external message body store. Bodies up to the configured inline size limit are stored in the database; larger bodies use the selected external store. SQL-backed Audit instances do not require an external message body store.

| External body storage | Use when | Configuration |
| --- | --- | --- |
| File system | A durable directory or mounted volume is available to the Error instance | [Configure file system storage](/servicecontrol/storage/external-body/file.md) |
| Azure Blob Storage | Message bodies should be stored in an Azure Storage account | [Configure Azure Blob Storage](/servicecontrol/storage/external-body/azure-blob.md) |
| Amazon S3 | Message bodies should be stored in an S3 bucket | [Configure Amazon S3 storage](/servicecontrol/storage/external-body/amazon-s3.md) |

See the [external message body storage overview](/servicecontrol/storage/external-body/) for shared configuration and retention guidance.

## Backup and restore

Back up and monitor each instance's database and, for SQL-backed Error instances, its external message body store. Restoring only the database or only the external bodies can leave message records without their bodies. Use the database and body-store providers' backup and availability guidance; the [RavenDB backup procedure](/servicecontrol/storage/ravendb/backup-sc-database.md) does not apply to SQL storage.

#end-if

Changing the storage provider of an existing instance is not the same as configuring a new instance. Do not use the provider settings as a data migration procedure.
