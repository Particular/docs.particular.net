---
title: External Message Body Storage
summary: Choose and configure external message body storage for SQL-backed ServiceControl Error instances.
reviewed: 2026-10-29
component: ServiceControl
versions: '[7,)'
---

SQL Server and PostgreSQL storage separate large message bodies from the Error instance's database. The database stores message metadata and small text bodies; an external store holds larger bodies and every binary body. Configure an external store even if most messages have small bodies.

The settings below use the Error instance configuration prefix. For Audit instances using external body storage, see the storage isolation guidance under [Operations](#operations). RavenDB manages message body storage itself.

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

A message body is stored in the database only when it is text and no larger than `ServiceControl/MaxBodySizeToStore`. The following bodies go to the external store, whatever their size:

- Bodies whose `NServiceBus.ContentType` header is missing, or does not contain `text/`, `xml`, or `json`, or contains `binary`
- Bodies that have a `Content-Encoding` header, such as compressed payloads
- Bodies that are not valid UTF-8 text, or that contain NUL characters

A text body larger than the limit is also stored externally, and its first `MaxBodySizeToStore` bytes are kept in the database so that full-text search can match it. When endpoints use a binary serializer, plan for every message body to be in the external store.

Bodies in the external store are compressed when they are at least `MessageBody/MinCompressionSize` bytes.

> [!NOTE]
> Audit instances have a setting with the same name, [`ServiceControl.Audit/MaxBodySizeToStore`](/servicecontrol/audit-instances/configuration.md#performance-tuning-servicecontrol-auditmaxbodysizetostore), that works differently. For Audit instances without an external body store, it is a hard limit: larger bodies cannot be viewed. For Error instances, bodies larger than `ServiceControl/MaxBodySizeToStore` are still stored, in the external message body store.

include: sc-error-maxbodysizetostore

include: sc-error-messagebody-mincompressionsize

See the [Error instance storage settings](/servicecontrol/servicecontrol-instances/configuration.md#storage) for the complete reference.

## Operations

Treat the database and external body store as a pair, and [back up and restore them together](/servicecontrol/storage/#backup-and-restore). Monitor body-store capacity and availability as well as database health.

> [!WARNING]
> Audit instances with different retention periods must not share an external body storage location. Retention cleanup deletes bodies by time prefix across the configured location, so an instance with shorter retention can delete bodies that another instance still needs. Message records can remain visible after their bodies are deleted. Configure a separate [Azure Blob Storage container](/servicecontrol/storage/external-body/azure-blob.md) or [S3 key prefix](/servicecontrol/storage/external-body/amazon-s3.md) for each Audit instance; the defaults do not isolate Audit instances from each other.

ServiceControl's retention process deletes a message's external body before it deletes the message's database record, starting with the oldest expired message. If body deletion fails, the affected records remain for a later sweep. The runtime identity therefore needs delete access as well as read and write access. After three failed runs in a row, the [**ServiceControl Retention**](/servicecontrol/servicecontrol-instances/#notifications-health-monitoring-sql-storage-retention) custom check reports the failure in ServicePulse.

> [!WARNING]
> Do not add storage cleanup rules, such as S3 Lifecycle rules or Azure Blob Storage lifecycle management policies, to the body store. ServiceControl deletes bodies itself when it removes their messages; rules that run on their own schedule delete bodies that ServiceControl still needs.

Changing the provider, directory, container, or bucket does not move existing message bodies. Do not use configuration changes as a migration procedure. For database choices, see the [storage overview](/servicecontrol/storage/).
