---
title: Compacting RavenDB 5 instances
summary: How to compact the RavenDB database backing ServiceControl for RavenDB 5 instances
reviewed: 2024-02-12
---

INFO: Compact the database only if the retention period, message throughput, or average message size have been reduced. If none of these have changed, compacting may not provide a significant reduction in database size, or it may have only a small, temporary effect.

INFO: The following documentation applies to ServiceControl error and audit instances using RavenDB 5 as the storage option. Audit instances created prior to version 4.26 and error instances prior to version 5.0 use RavenDB 3.5 and should follow a [different process](db-compaction.md).

ServiceControl's embedded RavenDB 5 database can be compacted by putting ServiceControl in maintenance mode, then following the [RavenDB process for compacting a database](https://ravendb.net/docs/article-page/5.4/csharp/studio/database/stats/storage-report).

## Using the RavenDB management portal

ServiceControl must be in maintenance mode in order to compact the database with the RavenDB management portal. While in maintenance mode, all ServiceControl features are disabled except RavenDB Studio and no messages are ingested from the queuing system.

### Step 1: Start ServiceControl in maintenance mode

* Start the ServiceControl instance in [maintenance mode](maintenance-mode.md).

### Step 2: Compact the database in RavenDB Studio

* Open RavenDB Studio: `http://localhost:{selected RavenDB port}/`. The default RavenDB port is 33334.
* Select Stats in the left menu, then Storage Report
* Ensure the correct ServiceControl database is selected in the dropdown in the top left
* Click "Compact database", then Compact
* Close the dialog when completed
* Take ServiceControl out of maintenance mode

### Step 3: Take ServiceControl out of maintenance mode

* In ServiceControl advanced options, selected "Stop Maintenance Mode"
