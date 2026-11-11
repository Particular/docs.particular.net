---
title: Compacting RavenDB instances
summary: How to compact the RavenDB database backing ServiceControl
reviewed: 2026-04-20
component: ServiceControl
redirects:
  - servicecontrol/db-compaction-v5
  - servicecontrol/db-compaction
---

If a ServiceControl instance's retention period, message throughput, or average message size have been reduced, it may be possible to compact the database. If none of these have changed, compacting may not provide a significant reduction in database size, or it may have only a small, temporary effect.

ServiceControl's RavenDB database can be compacted by [accessing the database](/servicecontrol/storage/ravendb/accessing-database.md), then following the [RavenDB process for compacting a database](https://docs.ravendb.net/5.4/studio/database/stats/storage-report).
