---
title: Migrating from RavenDB to SQL storage
summary: How to move ServiceControl Error and Audit instances from RavenDB to SQL Server or PostgreSQL storage
isUpgradeGuide: true
reviewed: 2026-11-24
component: ServiceControl
---

SQL Server and PostgreSQL storage are available from ServiceControl 7. Upgrading an existing instance to version 7 doesn't change its storage: it continues to use RavenDB with its existing data. How to move to SQL storage depends on the instance type:

| Instance | How to move to SQL storage |
| --- | --- |
| [Audit instance](#audit-instances) | Replace it with a new Audit instance that uses SQL storage. No data migration is needed. |
| [Error instance](#error-instances) | [Coming soon](https://github.com/Particular/ServiceControl/issues/6191), Keep using RavenDB until then. |

> [!WARNING]
> Do not change `StorageType` on an existing instance to switch to SQL storage. The data is not moved, and the instance starts with empty storage.

## Audit instances

Audit data isn't migrated. Audit instances delete audit messages when the [audit retention period](/servicecontrol/audit-instances/configuration.md#data-retention-servicecontrol-auditauditretentionperiod) ends, so instead of moving the data, replace the RavenDB Audit instance with a new Audit instance that uses SQL storage, and let the data in the old instance expire:

1. Upgrade the Error instance and the existing Audit instances to ServiceControl 7. See [Upgrade ServiceControl from version 6 to version 7](/servicecontrol/upgrades/6to7.md).
1. Create a new Audit instance that uses [SQL Server](/servicecontrol/storage/sqlserver/) or [PostgreSQL](/servicecontrol/storage/postgresql/) storage, and add it to the [remote instances](/servicecontrol/servicecontrol-instances/remotes.md) of the Error instance.
1. Disable audit queue ingestion on the old Audit instance. The old instance stops storing new audit messages, stays available for queries, and continues to delete audit messages when the retention period ends.
1. When the old Audit instance is empty, after the audit retention period, decommission it.

Until the old instance is decommissioned, the Error instance queries both Audit instances, so ServicePulse shows audit messages from both.

[Replacing an Audit instance](/servicecontrol/migrations/replacing-audit-instances/) describes each step for ServiceControl Management, PowerShell, and containers, including how to check that the old instance is empty.

## Error instances

Migration of Error instances from RavenDB to SQL Server or PostgreSQL storage is coming soon. It is not yet available.

Until then, keep Error instances on RavenDB. An Error instance that uses RavenDB can use Audit instances that use SQL storage, so Audit instances can be moved to SQL storage now.
