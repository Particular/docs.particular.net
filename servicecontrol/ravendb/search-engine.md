---
title: RavenDB search engine
summary: Selecting Corax or Lucene as the search engine for ServiceControl and how to migrate existing indexes between them
reviewed: 2026-10-02
component: ServiceControl
related:
- servicecontrol/ravendb/accessing-database
- servicecontrol/troubleshooting
- servicecontrol/upgrades/6.19to6.20
---

RavenDB supports two search engines for indexes: [Corax](https://docs.ravendb.net/indexes/search-engine/corax) and [Lucene](https://lucene.apache.org/). The selected engine determines how RavenDB builds and queries an index. The choice of engine is made when creating a database or index, and while it can be changed later, doing so triggers a full index rebuild.

The ServiceControl error and audit databases have a specific workload: messages are ingested continuously, expired messages are deleted continuously by the retention process, and the data is queried only occasionally, when ServicePulse is used. Load testing of this workload showed that, for the index definitions ServiceControl uses, Lucene indexes:

- take up less storage
- use less memory
- keep up with ingestion and deletion with less index lag
- query faster

Meanwhile, Corax demonstrated more performance and stability issues on large ServiceControl databases. Because of this, starting with ServiceControl version 6.20, new databases default to using Lucene, and **migrating existing databases to Lucene is recommended**.

## Default search engine per version

| ServiceControl version | Search engine for **new** databases | Existing databases |
|---|---|---|
| 5.x and 6.0–6.19 | Corax | Keep the engine they were created with |
| 6.20 and later | Lucene | Keep the engine they were created with |

As the table above illustrates, upgrading ServiceControl does not impact the search engine of existing databases. This is because changing the engine triggers a full rebuild of every index which, depending on the available computing power, can take days on very large databases. In addition, while the rebuild runs, the ingestion and indexing rates are degraded. Thus, migration should be planned and scheduled for each environment. See [Should existing indexes be migrated?](#should-existing-indexes-be-migrated) and [Migrating existing indexes to Lucene](#migrating-existing-indexes-to-lucene) for more details.

> [!NOTE]
> Monitoring instances do not use RavenDB and are not affected.

## Detecting indexes that use Corax

_Available in version 6.20_

Error and audit instances report indexes using Corax in two ways:

- A **custom check** named `Error Database Search Engine` (error instance) or `Audit Database Search Engine` (audit instance), visible in [ServicePulse](/servicepulse/). The check fails when at least one index uses Corax. It is evaluated hourly.
- A **warning** in the instance log at every start-up.

Both list the affected indexes and contain the following message:

> The following RavenDB index(es) use the Corax search engine: `<database>/<index>`. Lucene indexes are smaller, use less memory and perform better for ServiceControl workloads, and are the default for new databases. Consider switching these indexes to Lucene. Note that switching triggers a full rebuild of the index: on very large databases this can take days depending on the available compute, and while the rebuild is running ingestion and indexing rates can be degraded. Plan the switch accordingly.

The search engine of an index can also be inspected in RavenDB Studio, on the **Configuration** tab of the index, or in the **Indexes** list where each index shows its engine.

## Should existing indexes be migrated?

Yes. Migrating existing error and audit databases to Lucene is recommended for all instances. Corax has shown performance and stability issues on large ServiceControl databases, and these issues get worse as the database grows. Migrating proactively, while the database is small and the instance is healthy, keeps the rebuild short and efficient. This avoids a more complicated migration when the instance is already struggling.

Migrate as soon as possible when the instance shows one or more of the following symptoms. They indicate that the Corax indexes can no longer keep up with the load:

- Frequent or persistent index lag, reported by the [stale indexes](/servicecontrol/troubleshooting.md#stale-indexes) custom check
- High RAM utilization or [RavenDB dirty memory](/servicecontrol/troubleshooting.md#ravendb-dirty-memory) warnings
- [High CPU utilization](/servicecontrol/troubleshooting.md#high-cpu-utilization) caused by indexing
- Corrupted indexes or a lengthy database recovery after a service shutdown (see [Audit instances: Corrupted indexes or corrupted database after a service shutdown](/servicecontrol/troubleshooting.md#audit-instances-corrupted-indexes-or-corrupted-database-after-a-service-shutdown) for troubleshooting this problem)
- Database storage growth that is dominated by the size of the indexes

Instances without these symptoms should be migrated in the next planned maintenance window. Because the rebuild requires downtime or degraded ingestion, plan the migration separately for each environment. Migrate development and test instances first to estimate the rebuild duration for production. The `Error Database Search Engine` or `Audit Database Search Engine` custom check continues to fail until all indexes use Lucene.

> [!WARNING]
> Before migrating, [back up the database](/servicecontrol/backup-sc-database.md) and estimate the rebuild duration. The rebuild has to process every document in the database. Extrapolate from a smaller instance, or from the time the last [database upgrade](/servicecontrol/upgrades/) took, and schedule the migration in a maintenance window. Consider temporarily adding CPU and RAM to the host until the rebuild completes.

## Migrating existing indexes to Lucene

Do the migration for each index in RavenDB Studio.

> [!IMPORTANT]
> On ServiceControl versions before 6.21.1, also **lock** the migrated index after the migration. Those versions deploy their index definitions at each start-up. Without the lock, they reset the index to the database default, Corax, and start another rebuild.

The indexes with the highest load, and therefore the ones that benefit most, are:

| Instance | Index |
|---|---|
| Error | `FailedMessageViewIndex`, `MessagesViewIndex` |
| Audit | `MessagesViewIndex` or `MessagesViewIndexWithFullTextSearch` (depending on whether [full-text search on message bodies](/servicecontrol/audit-instances/configuration.md#performance-tuning-servicecontrol-auditenablefulltextsearchonbodies) is enabled) |

Other indexes can be migrated using the same procedure. Migrate one index at a time and wait for it to become non-stale before migrating the next one to limit the impact on ingestion.

### 1. Access the RavenDB Studio

- **Windows deployment**: Start the instance in [maintenance mode](/servicecontrol/ravendb/accessing-database.md#windows-deployment-maintenance-mode) and click **Launch RavenDB Studio**.
- **Container deployment**: Stop the ServiceControl container and open RavenDB Studio on port `8080` of the [database container](/servicecontrol/ravendb/containers.md).
- **External RavenDB server**: Open RavenDB Studio for the server that hosts the ServiceControl database.

Running the migration while the instance is stopped (maintenance mode) is recommended. It avoids ingestion competing with the rebuild for CPU and I/O and prevents the instance from resetting the index before it has been locked. Messages will accumulate in the error and audit queues while the instance is stopped; ensure the queues have enough capacity for the expected duration.

### 2. Change the search engine of the index

1. In RavenDB Studio, select the ServiceControl database and open **Indexes** > **List of Indexes**.
2. Click the index to edit it.
3. Open the **Configuration** tab.
4. Change **Search engine** from `Corax` or `Corax (inherited)` to `Lucene`.
5. Click **Save**.

RavenDB creates a replacement index that uses Lucene and runs side-by-side with the existing Corax index. The existing index will keep serving queries until the replacement has caught up.

### 3. Swap the indexes

In the **List of Indexes**, the index shows the replacement being built. Once the replacement is no longer stale, RavenDB swaps it in automatically and deletes the Corax index. RavenDB Studio also offers **swap now**:

- Swapping immediately frees the storage of the Corax index right away but queries return stale results until the Lucene index has been fully rebuilt.
- Waiting for the automatic swap keeps queries accurate but temporarily requires storage for both indexes.

> [!NOTE]
> Under constant ingestion the replacement index may never be reported as non-stale and the automatic swap may never happen. This is another reason to perform the migration while the instance is in maintenance mode, or to swap the indexes manually.

### 4. Lock the index

> [!NOTE]
> The lock is necessary on ServiceControl versions before 6.21.1. From version 6.21.1, ServiceControl keeps the search engine configured on an index when it updates its index definitions at start-up. On these versions, this step is not necessary.

While still in RavenDB Studio, click the `🔓 Unlocked` button of the migrated index and change it to `🔒 Locked (ignore)` ([lock modes](https://ravendb.net/docs/article-page/7.0/csharp/client-api/operations/maintenance/indexes/set-index-lock#lock-modes)). RavenDB Studio confirms with _Lock mode was set to: Locked (ignore)_.

A locked index is left untouched when ServiceControl recreates its index definitions at start-up, so the index stays on Lucene.

> [!WARNING]
> A locked index does not receive index definition changes from new ServiceControl versions. Check the upgrade guide of each new version for changes to the locked indexes. If an index definition changes, unlock the index, let ServiceControl update it, and do this migration again for it. On version 6.21.1 and later, the index can stay unlocked. ServiceControl then applies definition changes automatically and the index stays on Lucene.

#### Index reset to Corax after unlocking

On versions 6.20.0 to 6.21.0, ServiceControl resets an unlocked migrated index at the next start-up. RavenDB then builds a replacement index with the name `ReplacementOf/<index>` that uses Corax. When the replacement is no longer stale, RavenDB swaps it in. The recovery steps depend on the state of the swap. Check the **List of Indexes** in the Studio:

- **The replacement is not swapped in yet**: `ReplacementOf/<index>` is in the list and the migrated index shows Lucene. Upgrade to version 6.21.1 or later and make sure that the migrated index is unlocked. At start-up, ServiceControl keeps the Lucene search engine configured on the migrated index. RavenDB discards the Corax replacement and does not rebuild the migrated index. If an upgrade is not possible, stop the instance, lock the migrated index again as `🔒 Locked (ignore)`, and delete the `ReplacementOf/<index>` index in the Studio.
- **The replacement is swapped in**: `ReplacementOf/<index>` is not in the list and the index shows Corax. The Lucene configuration is lost. Upgrade to version 6.21.1 or later. Then do [step 2](#migrating-existing-indexes-to-lucene-2-change-the-search-engine-of-the-index) again for the index. On small or idle databases, the swap completes in seconds after start-up. This state is the more common one.

### 5. Restart the instance

Stop maintenance mode or start the ServiceControl container. The next start-up no longer logs a warning for the migrated index, and the `Error Database Search Engine` / `Audit Database Search Engine` custom check passes once all indexes use Lucene.

## Migrating the whole database

Instead of migrating indexes one by one, the database default can be changed so that all indexes, including future ones, use Lucene without locking:

1. In RavenDB Studio, open **Settings** > **Database Settings** for the ServiceControl database.
2. Set both `Indexing.Static.SearchEngineType` and `Indexing.Auto.SearchEngineType` to `Lucene` and save.
3. Reload the database when prompted.
4. **Reset** each index (**List of Indexes** > index menu > **Reset**) so that it is rebuilt with the new engine.

Resetting an index deletes it and rebuilds it from scratch; queries against it return stale results until the rebuild completes. Because all indexes are rebuilt, this approach causes a longer period of degraded performance than migrating individual indexes, but does not require indexes to be locked and applies to indexes added by future ServiceControl versions as well. Because the custom check only passes once all indexes use Lucene, this approach is the most direct way to complete the migration.
