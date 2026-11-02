---
title: External Message Body Storage
summary: Choose and configure external message body storage for SQL-backed ServiceControl Error and Audit instances.
reviewed: 2026-10-29
component: ServiceControl
---

SQL Server and PostgreSQL storage separate large message bodies from the instance's database. The database stores message metadata and small text bodies; an external store holds larger bodies and every binary body. Configure an external store even if most messages have small bodies.

This configuration is required for SQL-backed Error and Audit instances. RavenDB manages message body storage itself.

## Storage providers

Choose one provider per instance and follow its configuration guide.

| Provider | `MessageBody/StorageType` value | Requirements | Configuration |
| --- | --- | --- | --- |
| File system | `FileSystem` | A durable directory accessible to the instance | [Configure file system storage](/servicecontrol/storage/external-body/file.md) |
| Azure Blob Storage | `AzureBlob` | An Azure Storage account and credentials | [Configure Azure Blob Storage](/servicecontrol/storage/external-body/azure-blob.md) |
| Amazon S3 | `S3` | An S3 bucket and credentials | [Configure Amazon S3 storage](/servicecontrol/storage/external-body/amazon-s3.md) |

Configure the body store alongside [SQL Server](/servicecontrol/storage/sqlserver/) or [PostgreSQL](/servicecontrol/storage/postgresql/) storage. Use the same settings during setup and normal operation. Error instance settings use the `ServiceControl/` prefix, and Audit instance settings use the `ServiceControl.Audit/` prefix. The ServiceControl Management Utility and the PowerShell module configure body storage for Error instances only; configure Audit instances in the configuration file or with environment variables.

> [!WARNING]
> Give each instance its own directory, container, or key prefix, and do not share one between an Error instance and an Audit instance. Audit instances delete the bodies of expired messages by hour or by day across the whole location, so an instance with a shorter retention period can delete bodies that another instance still needs, and those messages stay visible without their bodies. The Audit defaults (`audit-bodies` and `audit-bodies/`) differ from the Error defaults, but they are the same for every Audit instance, so set a different [container](/servicecontrol/storage/external-body/azure-blob.md) or [key prefix](/servicecontrol/storage/external-body/amazon-s3.md) for each Audit instance.

include: sc-error-messagebody-storagetype

include: sc-audit-messagebody-storagetype

## Inline storage and compression

A message body is stored in the database only when it is text and no larger than `MaxBodySizeToStore`. The following bodies go to the external store, whatever their size:

- Bodies whose `NServiceBus.ContentType` header is missing, or does not contain `text/`, `xml`, or `json`, or contains `binary`
- Bodies that have a `Content-Encoding` header, such as compressed payloads
- Bodies that are not valid UTF-8 text, or that contain NUL characters

A text body larger than the limit is also stored externally, and its first `MaxBodySizeToStore` bytes are kept in the database so that full-text search can match it. When endpoints use a binary serializer, plan for every message body to be in the external store.

Bodies in the external store are compressed when they are at least `MessageBody/MinCompressionSize` bytes.

include: sc-error-maxbodysizetostore

include: sc-audit-maxbodysizetostore

include: sc-error-messagebody-mincompressionsize

include: sc-audit-messagebody-mincompressionsize

See the [Error instance storage settings](/servicecontrol/servicecontrol-instances/configuration.md#storage) and [Audit instance storage settings](/servicecontrol/audit-instances/configuration.md#storage) for the complete reference.

## Operations

Treat the database and external body store as a pair, and [back up and restore them together](/servicecontrol/storage/#backup-and-restore). Monitor body-store capacity and availability as well as database health.

An Error instance's retention process deletes a message's external body before it deletes the message's database record, starting with the oldest expired message. If body deletion fails, the affected records remain for a later sweep. After three failed runs in a row, the [**ServiceControl Retention**](/servicecontrol/servicecontrol-instances/#notifications-health-monitoring-sql-storage-retention) custom check reports the failure in ServicePulse.

Audit instances delete the external bodies of each expired hour (SQL Server) or day (PostgreSQL) before removing the corresponding database records. If the body store fails, the records stay and the next retention run tries again. Repeated failures are reported by the [**Audit retention**](/servicecontrol/audit-instances/#notifications-health-monitoring-sql-storage-retention) custom check.

Because retention deletes bodies, the identity the instance runs as needs delete access to the body store as well as read and write access.

> [!WARNING]
> Do not add storage cleanup rules, such as S3 Lifecycle rules or Azure Blob Storage lifecycle management policies, to the body store. ServiceControl deletes bodies itself when it removes their messages; rules that run on their own schedule delete bodies that ServiceControl still needs.

Changing the provider, directory, container, or bucket does not move existing message bodies. Do not use configuration changes as a migration procedure. For database choices, see the [storage overview](/servicecontrol/storage/).
