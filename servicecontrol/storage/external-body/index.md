---
title: External Message Body Storage
summary: Choose and configure external message body storage for SQL-backed ServiceControl Error instances.
reviewed: 2026-10-27
component: ServiceControl
versions: '[7,)'
---

SQL Server and PostgreSQL storage separate large message bodies from the Error instance's database. The database stores message metadata and smaller bodies; an external store holds bodies that exceed the inline size limit. Configure an external store even if most messages have small bodies.

This configuration applies only to SQL-backed Error instances. SQL-backed Audit instances do not require an external body store. RavenDB manages message body storage itself.

## Storage providers

Choose one provider per Error instance and follow its configuration guide.

| Provider | `ServiceControl/MessageBody/StorageType` | Requirements | Configuration |
| --- | --- | --- | --- |
| File system | `FileSystem` | A durable directory accessible to the instance | [Configure file system storage](/servicecontrol/storage/external-body/file.md) |
| Azure Blob Storage | `AzureBlob` | An Azure Storage account and credentials | [Configure Azure Blob Storage](/servicecontrol/storage/external-body/azure-blob.md) |
| Amazon S3 | `S3` | An S3 bucket and credentials | [Configure Amazon S3 storage](/servicecontrol/storage/external-body/amazon-s3.md) |

Configure the body store alongside [SQL Server](/servicecontrol/storage/sqlserver/) or [PostgreSQL](/servicecontrol/storage/postgresql/) storage. Use the same settings during setup and normal operation.

include: sc-error-messagebody-storagetype

## Inline storage and compression

Bodies larger than the inline size limit use the external store. Bodies in the external store are compressed above the minimum compression size.

include: sc-error-maxbodysizetostore

include: sc-error-messagebody-mincompressionsize

See the [Error instance storage settings](/servicecontrol/servicecontrol-instances/configuration.md#storage) for the complete reference.

## Operations

Treat the database and external body store as a pair, and [back up and restore them together](/servicecontrol/storage/#backup-and-restore). Monitor body-store capacity and availability as well as database health.

ServiceControl's retention sweeper deletes external bodies before removing their database records. If body deletion fails, the affected records remain for a later sweep. The runtime identity therefore needs delete access as well as read and write access.

> [!WARNING]
> Do not add storage cleanup rules, such as S3 Lifecycle rules or Azure Blob Storage lifecycle management policies, to the body store. ServiceControl deletes bodies itself when it removes their messages; rules that run on their own schedule delete bodies that ServiceControl still needs.

Changing the provider, directory, container, or bucket does not move existing message bodies. Do not use configuration changes as a migration procedure. For database choices, see the [storage overview](/servicecontrol/storage/).
