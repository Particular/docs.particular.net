---
title: External Message Body Storage
summary: Choose and configure external message body storage for SQL-backed ServiceControl Error instances.
reviewed: 2026-06-01
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

## Inline storage and compression

`ServiceControl/MaxBodySizeToStore` controls the maximum body size stored inline in the SQL database. The default is `102400` bytes (100 KB). Larger bodies use the external store.

`ServiceControl/MessageBody/MinCompressionSize` controls the minimum size at which external bodies are compressed. The default is `4096` bytes (4 KB).

| App config key | Environment variable | Default |
| --- | --- | --- |
| `ServiceControl/MaxBodySizeToStore` | `SERVICECONTROL_MAXBODYSIZETOSTORE` | `102400` bytes |
| `ServiceControl/MessageBody/MinCompressionSize` | `SERVICECONTROL_MESSAGEBODY_MINCOMPRESSIONSIZE` | `4096` bytes |

See the [Error instance storage settings](/servicecontrol/servicecontrol-instances/configuration.md#storage) for the complete reference.

## Operations

Treat the database and external body store as a pair. Back up and restore both together; restoring only one can leave database records without their message bodies. Monitor body-store capacity and availability as well as database health.

ServiceControl's retention sweeper deletes external bodies before removing their database records. If body deletion fails, the affected records remain for a later sweep. The runtime identity therefore needs delete access as well as read and write access.

Keep stored bodies available for as long as ServiceControl retains the corresponding messages. Do not apply independent cleanup policies that delete bodies before ServiceControl no longer needs them.

Changing the provider, directory, container, or bucket does not move existing message bodies. Do not use configuration changes as a migration procedure. For database choices, see the [storage overview](/servicecontrol/storage/).
