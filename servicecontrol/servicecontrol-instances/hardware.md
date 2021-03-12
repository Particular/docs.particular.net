---
title: ServiceControl Hardware Considerations
summary: Hardware recommendations for running ServiceControl
reviewed: 2020-07-07
---

ServiceControl as an application can be used to process the entire message load of a system. This article provides general guidelines, recommendations, and performance benchmarks to help determine the resources to provide for a production environment. To identify the hardware specifications for any system environment, a combination of testing with the system and the information provided below will need to be used.

## General recommendations

* Install ServiceControl on a dedicated server in production.
* 6 GB of RAM minimum
* 2 GHz quad core CPU or better
* [Database path](/servicecontrol/creating-config-file.md#host-settings-servicecontroldbpath) located on disks suitable for low latency write operations (fiber, solid state drives, raid 10), with a recommended IOPS of at least 7500.

NOTE: To ensure disk performance, use a benchmark tool, such as [CrystalDiskMark](https://crystalmark.info/en/software/crystaldiskmark/) (Simple) or [DiskSpd](https://github.com/Microsoft/diskspd) (Advanced).

### Server performance monitoring

Due to changes in the system it supports, the requirements for a server hosting ServiceControl can change over time. It is highly recommended that monitoring of the CPU, RAM, disk I/O, and network I/O for the server running ServiceControl be included.

Real disk, CPU, RAM, and network performance can be monitored with the Windows Resource Monitor and/or Windows Performance counters.

### Storage

It is recommended to:

- Store ServiceControl data on a dedicated disk. This makes low-level resource monitoring easy and ensures different applications are not competing for storage IOPS.
- Disable disk write caching to prevent data corruption if the (virtual) server or disk controler fails. This is a general best practice for databases.

## Suggestions to improve performance

### More RAM

The embedded RavenDB will utilize additional RAM to improve indexing performance.

### Message size / MaxBodySizeToStore

In general, the smaller the message, the quicker ServiceControl will be able to process audit records. Consider [using smaller messages](https://particular.net/blog/putting-your-events-on-a-diet). For larger message payloads, consider using the [data bus feature](/nservicebus/messaging/databus/).

In addition, for audit messages, lower the [`ServiceControl/MaxBodySizeToStore`](/servicecontrol/creating-config-file.md#performance-tuning-servicecontrolmaxbodysizetostore) setting to skip storage of larger audit messages. This setting will only reduce load if a non-binary [serialization](/nservicebus/serialization/) is used.

WARNING: When using ServiceInsight, the message body will not be viewable for messages that exceed the `ServiceControl/MaxBodySizeToStore` limit.

### Use a dedicated disk for the database

Use a dedicated disk for the ServiceControl [database path](/servicecontrol/creating-config-file.md#host-settings-servicecontroldbpath).

Additionally, it is possible to store the embedded database index files on a separate disk. Use the [`Raven/IndexStoragePath`](/servicecontrol/creating-config-file.md#host-settings-ravenindexstoragepath) setting to change the index storage location.

### Azure disk limitations

Using multiple 7500 IOPS disks in striped mode in Azure may not improve performance due to increased latency; consider [scaling out ServiceControl to multiple instances](#suggestions-to-improve-performance-scale-out).

### Scale out

If it is not possible to scale up ServiceControl to handle system volume, partition audit processing between multiple instances of ServiceControl. See [Multiple ServiceControl Instances](distributed-instances.md) for more details.
