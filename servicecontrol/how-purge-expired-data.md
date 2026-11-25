---
title: Automatic Expiration of Data
summary: Configuring ServiceControl's data retention policy
related:
 - nservicebus/recoverability
 - nservicebus/operations/auditing
 - servicecontrol/audit-instances
reviewed: 2026-11-24
---

ServiceControl stores audit and error data and deletes it once it is older than the retention period configured for the instance. The retention periods for both failed and audited messages must be set during installation. These values can be modified later by either

- Launching ServiceControl Management and editing the configuration settings for the instance
- Directly changing settings in configuration files
- Configuring environment variables when hosted in containers

The instance reads a changed retention period when it next starts. How the new value affects data that is already stored depends on the [storage type](/servicecontrol/storage/).

> [!NOTE]
> The expiration process curates only the data that ServiceControl stores: its database and, with SQL Server or PostgreSQL storage, its [message body store](/servicecontrol/storage/external-body/). Audit and error forwarding queues are not curated or managed by ServiceControl. To turn these settings off, edit the configuration settings for the instance.

## RavenDB storage

RavenDB gives each document an expiry time when ServiceControl writes it, based on the retention period at that moment. RavenDB deletes documents whose expiry time has passed, checking at the interval set by `ExpirationProcessTimerInSeconds` ([Error instances](/servicecontrol/servicecontrol-instances/configuration.md#ravendb-storage-servicecontrolexpirationprocesstimerinseconds), [Audit instances](/servicecontrol/audit-instances/configuration.md#ravendb-storage-servicecontrol-auditexpirationprocesstimerinseconds)).

When the retention period changes, the new value applies only to documents written afterwards. Documents already stored keep their expiry time.

> [!WARNING]
> The database will not automatically shrink in size after reducing the retention period. Ensure ServiceControl had time to purge all expired messages and then [compact the database](storage/ravendb/compaction.md).

## SQL Server and PostgreSQL storage

Each instance runs a retention sweep every hour. Each sweep works out the cutoff from the current retention period and deletes the data older than it. Message bodies in the external body store are deleted before the database rows that refer to them.

When the retention period changes, the new value applies to all stored data from the next sweep. Shortening the retention period deletes existing data that is older than the new period.

A sweep that keeps failing is reported by the [SQL storage retention custom checks](/servicecontrol/servicecontrol-instances/#notifications-health-monitoring-sql-storage-retention).
