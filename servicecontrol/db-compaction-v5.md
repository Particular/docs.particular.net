---
title: Compacting RavenDB 5 instances
summary: How to compact the RavenDB database backing ServiceControl for RavenDB 5 instances
reviewed: 2024-02-12
---

> [!NOTE]
> Compact the database only if the retention period, message throughput, or average message size have been reduced. If none of these have changed, compacting may not provide a significant reduction in database size, or it may have only a small, temporary effect.

> [!NOTE]
> The following documentation applies to ServiceControl error and audit instances using RavenDB 5 as the storage option. Audit instances created prior to version 4.26 and error instances prior to version 5.0 use RavenDB 3.5 and should follow a [different process](db-compaction.md).

ServiceControl's RavenDB 5 database can be compacted by [accessing the database](/servicecontrol/ravendb/accessing-database.md), then following the [RavenDB process for compacting a database](https://ravendb.net/docs/article-page/5.4/csharp/studio/database/stats/storage-report).