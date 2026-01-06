---
title: ServiceControl Hardware Considerations
summary: Hardware recommendations for running ServiceControl instances
component: ServiceControl
reviewed: 2024-05-06
---

This article provides recommendations and performance benchmarks to help select resources for a ServiceControl production environment.

## General recommendations

* A dedicated production server for installing ServiceControl instances (Error, Audit, and Monitoring).
* A minimum of 16 GB of RAM (excluding RAM for OS and other services).
* 2 GHz quad core CPU or better.
* A dedicated, non-virtual, pre-allocated SSD for ServiceControl databases (not the disk where the operating system is installed).

### Scaling ServiceControl

When possible, scaling *up* a single machine to handle system load is recommended. When scaling up is not an option, ServiceControl may be scaled *out* by partitioning audit processing between multiple instances. See [Multiple ServiceControl Instances](remotes.md) for more details.

### Ongoing server performance monitoring

The requirements for a server hosting ServiceControl may change over time as the system evolves. It's important to continuously monitor the CPU, RAM, disk I/O, and network I/O for the server running ServiceControl to ensure adequate resources are available for overall system health.

Disk, CPU, RAM, and network performance may be monitored using the Windows Resource Monitor and/or Windows Performance counters.

### Storage recommendations

* Store ServiceControl data on a dedicated disk. This makes low-level resource monitoring easier and ensures applications are not competing for storage IOPS.
* Store multiple ServiceControl databases on separate physical disks to prevent multiple instances competing for the same disk resources.
* Disable disk write caching (read caching can remain enabled) to prevent data corruption if the (virtual) server or disk controller fails. This is a general best practice for databases.
* [Database paths](/servicecontrol/servicecontrol-instances/configuration.md#embedded-database-servicecontroldbpath) should be located on disks suitable for low latency write operations (e.g. fiber, solid state drives, raid 10), with a recommended IOPS of at least 7500.
* Use fixed-size (not dynamically expanding virtual) disks
* Use solid state drives (SSDs) to significantly reduce seek times and increase throughput

> [!NOTE]
> To measure disk performance, use a storage benchmark tool such as Windows System Assessment Tool (`winsat disk -drive g`), [CrystalDiskMark](https://crystalmark.info/en/software/crystaldiskmark/), or [DiskSpd](https://github.com/Microsoft/diskspd).

> [!NOTE]
> Do not use an ephemeral AWS or Azure disk for ServiceControl data because these disks will be erased when the virtual machine reboots.

### Hosting in the cloud

ServiceControl can be hosted in the cloud by:

- Using a virtual machine
- Using a container hosting service.

> [!WARNING]
> Due to [RavenDB networked disk limitations](https://ravendb.net/docs/article-page/6.0/csharp/start/installation/running-in-docker-container#requirements) there may be [difficulties running the RavenDB container in the cloud with PaaS services](https://github.com/Particular/ServiceControl/issues/3340#issuecomment-2313694640) like [Azure Container Instances](https://azure.microsoft.com/en-us/products/container-instances) or [AWS Elastic Container Service](https://aws.amazon.com/ecs/). ServiceControl containers are compatible with [RavenDB Cloud](https://ravendb.net/cloud).

## Improving performance

### Increase RAM

The embedded RavenDB will use additional RAM to improve indexing performance. During times of high load, ServiceControl can peak to 12GB or more.

### Message size / MaxBodySizeToStore

In general, [the smaller the messages](https://particular.net/blog/putting-your-events-on-a-diet), the faster ServiceControl will process audit records. For larger message payloads, consider using the [data bus feature](/nservicebus/messaging/claimcheck/).

For audit messages, lower the [`ServiceControl.Audit/MaxBodySizeToStore`](/servicecontrol/audit-instances/configuration.md#performance-tuning-servicecontrol-auditmaxbodysizetostore) setting to skip storage of larger audit messages. This setting will only reduce load if non-binary [serialization](/nservicebus/serialization/) is used.

> [!WARNING]
> When using ServicePulse, the message body is not viewable for messages that exceed the `ServiceControl/MaxBodySizeToStore` limit.

### Separate disks for database and index files

Besides using a dedicated disk for the ServiceControl [database paths](/servicecontrol/servicecontrol-instances/configuration.md#embedded-database-servicecontroldbpath), it's possible to store the embedded database index files on a separate disk.

#if-version [5,)

Use [symbolic links (soft links) to map any RavenDB storage subfolder](https://ravendb.net/docs/article-page/5.4/csharp/server/storage/customizing-raven-data-files-locations) to other physical drives.

#end-if
#if-version [,5)

> [!NOTE]
> Only applies to instances that use the RavenDB 3.5 storage engine

Use the [`Raven/IndexStoragePath`](/servicecontrol/servicecontrol-instances/configuration.md?version=servicecontrol_4#embedded-database-ravenindexstoragepath) setting to change the index storage location.

#end-if

### Azure disk limitations

Using multiple 7500 IOPS disks in striped mode in Azure may not improve performance due to increased latency; consider [scaling out ServiceControl to multiple instances](#general-recommendations-scaling-servicecontrol) instead.

### Turn off full-text search

Updating the full-text index requires a considerable amount of CPU and disk space. If the full-text search on message bodies is not required, consider turning it off by doing either one of the following:

- Turn off the 'FULL TEXT SEARCH ON MESSAGE BODIES' in the settings configuration of ServiceControl Management Utility
- Modify the [ServiceControl.Audit/EnableFullTextSearchOnBodies](/servicecontrol/audit-instances/configuration.md#performance-tuning-servicecontrol-auditenablefulltextsearchonbodies) setting in the configuration file
