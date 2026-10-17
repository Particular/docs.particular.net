---
title: Upgrade Tips
summary: Tips for upgrading to newer versions of ServiceControl.
reviewed: 2026-10-17
related:
---

1. Ensure the machine that hosts ServiceControl has sufficient CPU and RAM resources during upgrades. Consider temporarily adding more vCPU and RAM until the upgrade completes and afterwards trim down the resources.
1. Updating ServiceControl will stop ingesting error/audit queue messages but does not affect the sending of messages between endpoints. While an update is in progress, it's not possible to trigger retries manually via ServicePulse. Consider [multiple (remote) instances](/servicecontrol/servicecontrol-instances/remotes.md) if uptime of ServicePulse is important.
1. Compacting the database will not make the update run faster. Compacting should only be considered *after* upgrading.
1. In-place updates are recommended over side-by-side updates as they have less complexity. Consider side-by-side if ServicePulse downtime must be minimized.
1. [Backup the database](/servicecontrol/backup-sc-database.md) before upgrading.
1. Account for downtime of ServicePulse as failed messages cannot be retried when ServiceControl is not running.
1. Account for queue storage size. While ServiceControl is down, messages are not ingested but they will still build up in the queue. If the system has a high message throughput the queues act as a buffer. Ensure queues will not run out of storage (quota) space as when queues are full no new messages can be added which will cause application outage.
1. Analyze the storage size for each ServiceControl instance and update starting from the smallest to the largest. This helps understand how smaller migrations run and can be used to extrapolate how long updating the larger instances will take which can help decided whether a side-by-side upgrade is necessary.

> [!NOTE]
> Upgrading the ServiceControl will not upgrade each of the individual error, monitoring or audit instances. Each of those instances will need to be upgraded separately if desired.

## Upgrades that rebuild indexes

Some ServiceControl versions change the definition of one or more RavenDB indexes. The [upgrade guide](/servicecontrol/upgrades/) for the version notes when this happens. At the first start-up after the upgrade, ServiceControl deploys the new definition and RavenDB builds it as a side-by-side replacement index, listed as `ReplacementOf/<index>` in RavenDB Studio. The existing index keeps serving queries while the replacement is built. Once the replacement has caught up with all documents and is no longer stale, RavenDB swaps it in and deletes the old index.

The rebuild has to process every document in the database. On large databases it can take a long time and it competes with message ingestion for CPU, RAM, and storage I/O. While ingestion continues, the rebuild takes longer, ingestion and indexing rates are degraded, and the replacement index might never be reported as non-stale, which prevents the automatic swap.

> [!WARNING]
> Until the swap, the database stores both the existing index and its replacement. The replacement usually requires at least as much storage as the index it replaces. Before upgrading, check the index sizes in RavenDB Studio and ensure the database drive has enough free space for both. If the drive runs low on storage, ServiceControl [stops ingesting messages](/servicecontrol/troubleshooting.md#low-on-storage-space).

### Upgrading audit instances one at a time

When audit messages are spread across [multiple audit instances](/servicecontrol/servicecontrol-instances/remotes.md#overview-sharding-audit-messages-with-competing-consumers), upgrade them one at a time so that the rebuild does not have to compete with ingestion and the environment keeps running:

1. Pick an audit instance and [disable audit message ingestion](/servicecontrol/audit-instances/configuration.md#recoverability-servicecontrol-auditingestauditmessages) by setting `ServiceControl.Audit/IngestAuditMessages` to `false`. The instance keeps serving its data to ServicePulse but no longer adds documents to its database and indexes. With competing consumers, the other audit instances keep ingesting from the audit queue.
1. Upgrade the instance. At start-up, RavenDB starts building the replacement indexes.
1. Wait until the rebuild completes. In RavenDB Studio, the **List of Indexes** must no longer show `ReplacementOf/<index>` entries, and the [stale indexes](/servicecontrol/troubleshooting.md#stale-indexes) custom check must pass. See [accessing the ServiceControl database](/servicecontrol/ravendb/accessing-database.md) for how to open RavenDB Studio.
1. Enable ingestion again by setting `ServiceControl.Audit/IngestAuditMessages` to `true`, or by removing the setting, and restart the instance.
1. Repeat with the next audit instance.

Start with the smallest instance to estimate how long the rebuild takes on the larger ones.

> [!NOTE]
> When each audit instance ingests from its own audit queue, for example with [split audit queues](/servicecontrol/servicecontrol-instances/remotes.md#overview-sharding-audit-messages-with-split-audit-queues), messages build up in the queue of the instance that has ingestion disabled until ingestion is enabled again. Ensure the queue does not run out of storage space. The same applies to an environment with a single audit instance.

For error instances, the equivalent setting is [`ServiceControl/IngestErrorMessages`](/servicecontrol/servicecontrol-instances/configuration.md#recoverability-servicecontrolingesterrormessages). While error ingestion is disabled, failed messages build up in the error queue.

## Downgrading ServiceControl

This section contains information about downgrading ServiceControl instances and the ServiceControl Management Utility.

### Instances

ServiceControl instances cannot be downgraded with the ServiceControl Management Utility. This is because the storage schema could have changed during a previous upgrade.

> [!NOTE]
> If a downgrade MUST be performed ensure that the storage schema is compatible.

> [!NOTE]
> It is recommended to execute this procedure under the supervision of a Particular software support engineer.

1. Open the ServiceControl Management Utility
2. Scroll to the instance that requires downgrading
3. Open the "Installation path"
4. Copy the `ServiceControl.exe.config` and store the identical instance name
5. Delete the instance but keep the database files
6. Close ServiceControl Management Utility
7. [Downgrade the installer](#downgrading-servicecontrol-installer)
8. Add a new instance using the same identical name and configuration as the deleted instance

### Installer

There may be a need to setup a ServiceControl instance for an older version of ServiceControl, in which case follow these steps:

1. Uninstall the current (newer) version from Windows in "Add/remove programs"
2. Install the previous version

The ServiceControl Management Utility (SCMU) can now be used again to add instances based on the older version.

> [!NOTE]
> Downgrading the setup can results in SCMU not being able to manage newer instance versions.
