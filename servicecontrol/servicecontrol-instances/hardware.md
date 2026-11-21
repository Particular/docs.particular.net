---
title: ServiceControl Hardware Considerations
summary: Hardware recommendations for running ServiceControl instances
component: ServiceControl
reviewed: 2026-10-27
---

This article provides recommendations for the machines that run ServiceControl instances. What a machine needs depends on where the instance stores its data:

- With [embedded RavenDB](#embedded-ravendb), as deployed by the ServiceControl Management Utility and PowerShell, the instance runs the database on the same machine, so that machine must be sized and provisioned as a database server.
- With [SQL Server, PostgreSQL, or an external RavenDB server](#external-databases), including the RavenDB container, the database runs on its own server or service. The ServiceControl machine runs only the instance.
- Monitoring instances do not store data and do not need a database.

## General recommendations

* Use a dedicated set of production servers, or container hosts, for ServiceControl instances (Error, Audit, and Monitoring).

> [!IMPORTANT]
> It's recommended to not install more than one instance of each [type](/servicecontrol/#servicecontrol-instance-types) (Error, Audit, and Monitoring) per server.

### Scaling ServiceControl

When possible, scale *up* a single machine to handle system load. When scaling up is not an option, ServiceControl may be scaled *out* by partitioning audit processing between multiple instances. See [Multiple ServiceControl Instances](remotes.md) for more details.

### Ongoing server performance monitoring

The requirements for a server hosting ServiceControl may change over time as the system evolves. It's important to continuously monitor the CPU, RAM, disk I/O, and network I/O for the server running ServiceControl to ensure adequate resources are available for overall system health.

Disk, CPU, RAM, and network performance may be monitored using the Windows Resource Monitor and/or Windows Performance counters.

### Hosting in the cloud

ServiceControl can be hosted in the cloud by:

- Using a virtual machine
- Using a container hosting service.

> [!WARNING]
> Due to [RavenDB networked disk limitations](https://docs.ravendb.net/6.2/start/installation/deployment-considerations#storage-considerations) there may be [difficulties running the RavenDB container in the cloud with PaaS services](https://github.com/Particular/ServiceControl/issues/3340#issuecomment-2313694640) like [Azure Container Instances](https://azure.microsoft.com/en-us/products/container-instances) or [AWS Elastic Container Service](https://aws.amazon.com/ecs/). ServiceControl containers are compatible with [RavenDB Cloud](https://ravendb.net/cloud).

With SQL Server or PostgreSQL storage, the database can be a managed database service from the cloud provider; see [external databases](#external-databases).

## Embedded RavenDB

These recommendations apply to Error and Audit instances that run embedded RavenDB. The disk recommendations also apply to the machine that runs an external RavenDB server or the RavenDB container.

* A minimum of 16 GB of RAM (excluding RAM for OS and other services).
* 3 GHz quad-core CPU or better.
* A dedicated, non-virtual and non-ephemeral, pre-allocated SSD for ServiceControl RavenDB databases (not the disk where the operating system is installed).

### Storage recommendations

* Store ServiceControl data on a dedicated disk. This makes low-level resource monitoring easier and ensures applications are not competing for storage IOPS.
* Store multiple ServiceControl databases on separate physical disks to prevent multiple instances competing for the same disk resources.
* Disable disk write caching (read caching can remain enabled) to prevent data corruption if the (virtual) server or disk controller fails. This is a general best practice for databases.
* [Database paths](/servicecontrol/servicecontrol-instances/configuration.md#storage-servicecontroldbpath) should be located on disks suitable for low-latency write operations (e.g., fiber, solid-state drives, RAID 10), with a recommended IOPS of at least 7500.
* Use fixed-size (not dynamically expanding virtual) disks
* Use solid-state drives (SSDs) to significantly reduce seek times and increase throughput
* Database compaction requires an amount of free disk space equal to the database to compact; account for the compaction operation when determining storage disk sizes

> [!NOTE]
> To measure disk performance, use a storage benchmark tool such as Windows System Assessment Tool (`winsat disk -drive g`), [CrystalDiskMark](https://crystalmark.info/en/software/crystaldiskmark/), or [DiskSpd](https://github.com/Microsoft/diskspd).

> [!NOTE]
> Do not use an ephemeral AWS or Azure disk for ServiceControl data because these disks will be erased when the virtual machine reboots.

### Message ingestion performance baseline

When using a virtual machine with the following hardware specs:

- 4 cores with hyperthreading (e.g, L4 or E4 series in Azure)
- 32 GB of RAM
- A dedicated premium SSD with 6400 IOPS and a max throughput of 250 MBps

With RavenDB storage, it's reasonable to expect that the ServiceControl instance can ingest up to 250 msgs/sec for a database of 1TB.

### Increase RAM

The embedded RavenDB will use additional RAM to improve indexing performance. During times of high load, ServiceControl can peak to 12GB or more.

### Separate disks for database, index, and journal files

> [!NOTE]
> This suggestion applies to ServiceControl Error instances. Audit instances have three indexes, and only one is performance-critical; in this case, the performance gain from using multiple disks is not worth the cost. 

Besides using a dedicated disk for the ServiceControl [database paths](/servicecontrol/servicecontrol-instances/configuration.md#storage-servicecontroldbpath), it's possible to store the embedded RavenDB index files on a separate disk.

Use [symbolic links (soft links) to map any RavenDB storage subfolder](https://ravendb.net/docs/article-page/5.4/csharp/server/storage/customizing-raven-data-files-locations) to other physical drives.

### Azure disk limitations

Using multiple 7500 IOPS disks in striped mode in Azure may not improve performance due to increased latency; consider [scaling out ServiceControl to multiple instances](#general-recommendations-scaling-servicecontrol) instead.

## External databases

When an Error or Audit instance uses SQL Server, PostgreSQL, or an external RavenDB server, the database does not run on the ServiceControl machine:

* Size, monitor, and back up the database server following the database vendor's guidance, or use a managed database service from the cloud provider.
* Keep the database close to the ServiceControl instance, in the same region or data center, as every ingested message is written to it.
* With SQL Server and PostgreSQL storage, also plan capacity for the [external message body store](/servicecontrol/storage/external-body/).
* With an external RavenDB server, follow the [embedded RavenDB disk recommendations](#embedded-ravendb-storage-recommendations) for the machine that runs the database.

The hardware figures and performance baseline in this article were measured with embedded RavenDB and do not apply to SQL Server or PostgreSQL storage. [Monitor the ServiceControl machine](#general-recommendations-ongoing-server-performance-monitoring) and the database server, and scale either one as needed.

## Improving performance

### Message size / MaxBodySizeToStore

In general, [the smaller the messages](https://particular.net/blog/putting-your-events-on-a-diet), the faster ServiceControl will process audit records. For larger message payloads, consider using the [data bus feature](/nservicebus/messaging/claimcheck/).

For audit messages, lower the [`ServiceControl.Audit/MaxBodySizeToStore`](/servicecontrol/audit-instances/configuration.md#performance-tuning-servicecontrol-auditmaxbodysizetostore) setting to skip storage of larger audit messages. This setting will only reduce load if non-binary [serialization](/nservicebus/serialization/) is used. With SQL Server and PostgreSQL storage, larger bodies are stored in the [external message body store](/servicecontrol/storage/external-body/) instead and remain viewable.

> [!WARNING]
> With RavenDB storage, ServicePulse cannot show the message body for messages that exceed the `MaxBodySizeToStore` limit.

### Turn off full-text search

> [!NOTE]
> This section applies only to RavenDB storage. SQL Server and PostgreSQL storage always enable full-text search and ignore `EnableFullTextSearchOnBodies`.

Updating the full-text index requires a considerable amount of CPU and disk space. If the full-text search on message bodies is not required, consider turning it off by doing either one of the following:

- Turn off the 'FULL TEXT SEARCH ON MESSAGE BODIES' in the settings configuration of ServiceControl Management Utility
- Modify the [ServiceControl.Audit/EnableFullTextSearchOnBodies](/servicecontrol/audit-instances/configuration.md#performance-tuning-servicecontrol-auditenablefulltextsearchonbodies) setting in the configuration file

### Tune ingestion concurrency and writers

Ingestion throughput depends on the transport, storage, and message mix, so there is no universal setting that is guaranteed to be faster. Test changes with a representative load, and monitor CPU, RAM, disk, network, and index lag before and after each change.

The following example raises the concurrency level and the number of parallel writers. The parallel writer settings are available from ServiceControl 7. Error instances support concurrent writers only with SQL Server and PostgreSQL storage; RavenDB error instances always use one writer.

```bash
# Error instance (SQL Server or PostgreSQL storage only)
SERVICECONTROL_MAXIMUMCONCURRENCYLEVEL=100
SERVICECONTROL_ERRORINGESTIONMAXPARALLELWRITERS=4

# Audit instance
SERVICECONTROL_AUDIT_MAXIMUMCONCURRENCYLEVEL=100
SERVICECONTROL_AUDIT_AUDITINGESTIONMAXPARALLELWRITERS=4
```

The batch size is not set, so each writer uses a batch size of `25` (concurrency divided by writers), and the batch timeout keeps its default of 100 milliseconds. More concurrency and writers increase resource pressure on the server and storage. Custom message enrichers must be thread-safe when multiple writers are used. See the [Error instance performance settings](/servicecontrol/servicecontrol-instances/configuration.md#performance-tuning) and [Audit instance performance settings](/servicecontrol/audit-instances/configuration.md#performance-tuning) for details.

### Persistently high disk I/O levels on audit instances

> [!NOTE]
> Starting with ServiceControl Version 6.20.0, prefetching is disabled by default.

Audit instances with full-text indexing and message expiration enabled, and affected by constant high throughput on the audit queue, might show persistently high disk I/O levels. Some of the I/O is caused by document prefetching to optimize query performance. Considering that, under those premises, there is a disproportionate amount of I/O time dedicated to writes, it is better to disable prefetching by setting the system-wide environment variable `RAVEN_Storage_EnablePrefetching` to `false`. Once set, restart the ServiceControl instances.

Once instances have been restarted, validate the setting has been applied by going to instance settings -> Database Settings -> Filter by `EnablePrefetching`, the reported value is `false`.
