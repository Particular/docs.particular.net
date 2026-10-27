---
title: ServiceControl RavenDB Storage
summary: RavenDB storage for ServiceControl Error and Audit instances.
reviewed: 2026-10-27
component: ServiceControl
---

RavenDB is the default storage for ServiceControl Error and Audit instances. Use this page to find RavenDB-specific deployment and maintenance guidance.

#if-version [7,)

From ServiceControl 7, new Error and Audit instances can also select [SQL Server](/servicecontrol/storage/sqlserver/) or [PostgreSQL](/servicecontrol/storage/postgresql/); see the [storage overview](/servicecontrol/storage/) for the differences.

#end-if

When deploying with PowerShell or the ServiceControl Management Utility, ServiceControl uses an embedded RavenDB database. For containers, use the [ServiceControl RavenDB container](/servicecontrol/storage/ravendb/containers.md) or a compatible external RavenDB server. Configure the connection for each [Error instance](/servicecontrol/servicecontrol-instances/deployment/containers.md) or [Audit instance](/servicecontrol/audit-instances/deployment/containers.md) separately.

## RavenDB operations

- [Choose the embedded database location](/servicecontrol/storage/ravendb/configure-ravendb-location.md).
- [Access the database](/servicecontrol/storage/ravendb/accessing-database.md).
- [Back up and restore the database](/servicecontrol/storage/ravendb/backup-sc-database.md).
- [Compact the database](/servicecontrol/storage/ravendb/compaction.md).
- [Move the database location](/servicecontrol/storage/ravendb/move-database-location.md).
- [Configure the RavenDB search engine](/servicecontrol/storage/ravendb/search-engine.md).

These procedures apply only to RavenDB storage.

#if-version [7,)

Configuring a different provider for an existing instance is not a data migration procedure.

#end-if
