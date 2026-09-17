---
title: RavenDB search engine
summary: Which RavenDB search engine ServiceControl uses for its indexes and how to migrate existing indexes from Corax to Lucene
reviewed: 2026-09-17
component: ServiceControl
related:
- servicecontrol/ravendb/accessing-database
- servicecontrol/troubleshooting
- servicecontrol/upgrades/6.19to6.20
---

RavenDB supports two search engines for indexes: [Corax](https://ravendb.net/docs/article-page/7.0/csharp/indexes/search-engine/corax) and [Lucene](https://lucene.apache.org/). The search engine determines how RavenDB builds and queries an index. Which engine is used is decided when a database or index is created and it can be changed afterwards, at the cost of a full index rebuild.

The ServiceControl error and audit databases have a specific workload: messages are ingested continuously, expired messages are deleted continuously by the retention process, and the data is queried only occasionally, when ServicePulse is used. Load testing of this workload showed that, for the index definitions ServiceControl uses, Lucene indexes:

- take up less storage
- use less memory
- keep up with ingestion and deletion with less index lag
- query faster

Corax has shown performance and stability issues on large ServiceControl databases. Starting with ServiceControl version 6.20, new databases therefore use Lucene, and **migrating existing databases to Lucene is recommended**.

## Default search engine per version

| ServiceControl version | Search engine for **new** databases | Existing databases |
|---|---|---|
| 5.x and 6.0–6.19 | Corax | Keep the engine they were created with |
| 6.20 and later | Lucene | Keep the engine they were created with |

Upgrading ServiceControl never changes the search engine of an existing database. Changing the engine triggers a full rebuild of every index, which on very large databases can take days depending on the available compute, and while the rebuild runs the ingestion and indexing rates are degraded. The upgrade therefore does not migrate existing databases automatically. The migration is a planned operator action that should be scheduled separately for each environment, see [Should existing indexes be migrated?](#should-existing-indexes-be-migrated) and [Migrating existing indexes to Lucene](#migrating-existing-indexes-to-lucene).

> [!NOTE]
> Monitoring instances do not use RavenDB and are not affected.

## Detecting indexes that use Corax

_Available in version 6.20_

Error and audit instances report indexes that still use Corax in two ways:

- A **custom check** named `Error Database Search Engine` (error instance) or `Audit Database Search Engine` (audit instance), visible in [ServicePulse](/servicepulse/). The check fails while at least one index uses Corax and passes once all indexes use Lucene. It is evaluated hourly.
- A **warning** in the instance log at every start-up.

Both list the affected indexes and contain the following message:

> The following RavenDB index(es) use the Corax search engine: `<database>/<index>`. Lucene indexes are smaller, use less memory and perform better for ServiceControl workloads, and are the default for new databases. Consider switching these indexes to Lucene. Note that switching triggers a full rebuild of the index: on very large databases this can take days depending on the available compute, and while the rebuild is running ingestion and indexing rates can be degraded. Plan the switch accordingly.

The search engine of an index can also be inspected in the RavenDB Studio, on the **Configuration** tab of the index, or in the **Indexes** list where each index shows its engine.

## Should existing indexes be migrated?

Yes. Migrating existing error and audit databases to Lucene is recommended for all instances. Corax has shown performance and stability issues on large ServiceControl databases, and these issues get worse as the database grows. Migrating proactively, while the database is small and the instance is healthy, keeps the rebuild short and avoids having to migrate while the instance is already struggling.

Migrate as soon as possible when the instance shows one or more of the following symptoms. They indicate that the Corax indexes can no longer keep up with the load:

- Frequent or persistent index lag, reported by the [stale indexes](/servicecontrol/troubleshooting.md#stale-indexes) custom check
- High RAM utilization or [RavenDB dirty memory](/servicecontrol/troubleshooting.md#ravendb-dirty-memory) warnings
- [High CPU utilization](/servicecontrol/troubleshooting.md#high-cpu-utilization) caused by indexing
- Corrupted indexes or a lengthy database recovery after a service shutdown, see [Audit instances: Corrupted indexes or corrupted database after a service shutdown](/servicecontrol/troubleshooting.md#audit-instances-corrupted-indexes-or-corrupted-database-after-a-service-shutdown)
- Database storage growth that is dominated by the size of the indexes

Instances without these symptoms should be migrated in the next planned maintenance window. Because the rebuild requires downtime or degraded ingestion, plan the migration separately for each environment. Migrate development and test instances first to estimate the rebuild duration for production. The `Error Database Search Engine` or `Audit Database Search Engine` custom check continues to fail until all indexes use Lucene.

> [!WARNING]
> Before migrating, [back up the database](/servicecontrol/backup-sc-database.md) and estimate the rebuild duration. The rebuild has to process every document in the database. Extrapolate from a smaller instance, or from the time the last [database upgrade](/servicecontrol/upgrades/) took, and schedule the migration in a maintenance window. Consider temporarily adding CPU and RAM to the host until the rebuild completes.

## Migrating existing indexes to Lucene

The migration is performed per index in the RavenDB Studio. On ServiceControl versions before 6.20, the migrated index must also be **locked** afterwards. Those versions recreate their index definitions at every start-up and would otherwise reset the index to the database default (Corax) and trigger another rebuild.

The indexes with the highest load, and therefore the ones that benefit most, are:

| Instance | Index |
|---|---|
| Error | `FailedMessageViewIndex`, `MessagesViewIndex` |
| Audit | `MessagesViewIndex` or `MessagesViewIndexWithFullTextSearch` (depending on whether [full-text search on message bodies](/servicecontrol/audit-instances/configuration.md#performance-tuning-servicecontrol-auditenablefulltextsearchonbodies) is enabled) |

Other indexes can be migrated using the same procedure. Migrate one index at a time and wait for it to become non-stale before migrating the next one to limit the impact on ingestion.

### 1. Access the RavenDB Studio

- **Windows deployment**: start the instance in [maintenance mode](/servicecontrol/ravendb/accessing-database.md#windows-deployment-maintenance-mode) and click **Launch RavenDB Studio**.
- **Container deployment**: stop the ServiceControl container and open the Studio on port `8080` of the [database container](/servicecontrol/ravendb/containers.md).
- **External RavenDB server**: open the Studio of the RavenDB server that hosts the ServiceControl database.

Running the migration while the instance is stopped (maintenance mode) is recommended. It avoids ingestion competing with the rebuild for CPU and I/O and prevents the instance from resetting the index before it has been locked. Messages accumulate in the error and audit queues while the instance is stopped; ensure the queues have enough capacity for the expected duration.

### 2. Change the search engine of the index

1. In the Studio, select the ServiceControl database and open **Indexes** > **List of Indexes**.
2. Click the index to edit it.
3. Open the **Configuration** tab.
4. Change **Search engine** from `Corax` or `Corax (inherited)` to `Lucene`.
5. Click **Save**.

RavenDB now creates a replacement index that uses Lucene next to the existing Corax index. The existing index keeps serving queries until the replacement has caught up.

### 3. Swap the indexes

In the **List of Indexes** the index shows the replacement being built. Once the replacement is no longer stale, RavenDB swaps it in automatically and deletes the Corax index. The Studio also offers to **swap now**:

- Swapping immediately frees the storage of the Corax index right away but queries return stale results until the Lucene index has been fully rebuilt.
- Waiting for the automatic swap keeps queries accurate but temporarily requires storage for both indexes.

> [!NOTE]
> Under constant ingestion the replacement index may never be reported as non-stale and the automatic swap may never happen. This is another reason to perform the migration while the instance is in maintenance mode, or to swap the indexes manually.

### 4. Lock the index

> [!NOTE]
> Locking the index is required to keep a non-default index configuration on ServiceControl versions prior to 6.20.0. On version 6.20.0 and later, this step can be skipped.

While still in the Studio, click the `🔓 Unlocked` button of the migrated index and change it to `🔒 Locked (ignore)` ([lock modes](https://ravendb.net/docs/article-page/7.0/csharp/client-api/operations/maintenance/indexes/set-index-lock#lock-modes)). The Studio confirms with _Lock mode was set to: Locked (ignore)_.

A locked index is left untouched when ServiceControl recreates its index definitions at start-up, so the index stays on Lucene.

> [!WARNING]
> Locking an index also means that index definition changes shipped with future ServiceControl versions are not applied to it. Check the upgrade guide of each new version for changes to the locked indexes; if an index definition changes, unlock the index, let ServiceControl update it, and repeat this migration for it.

### 5. Restart the instance

Stop maintenance mode or start the ServiceControl container. The next start-up no longer logs a warning for the migrated index, and the `Error Database Search Engine` / `Audit Database Search Engine` custom check passes once all indexes use Lucene.

## Migrating the whole database

Instead of migrating indexes one by one, the database default can be changed so that all indexes, including future ones, use Lucene without locking:

1. In the Studio open **Settings** > **Database Settings** of the ServiceControl database.
2. Set both `Indexing.Static.SearchEngineType` and `Indexing.Auto.SearchEngineType` to `Lucene` and save.
3. Reload the database when prompted.
4. **Reset** each index (**List of Indexes** > index menu > **Reset**) so that it is rebuilt with the new engine.

Resetting an index deletes it and rebuilds it from scratch; queries against it return stale results until the rebuild completes. Because all indexes are rebuilt, this approach causes a longer period of degraded performance than migrating individual indexes, but does not require indexes to be locked and applies to indexes added by future ServiceControl versions as well. Because the custom check only passes once all indexes use Lucene, this approach is the most direct way to complete the migration.
