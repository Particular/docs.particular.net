---
title: ServiceControl Hardware Considerations
summary: Hardware recommendations for running ServiceControl
reviewed: 2018-10-17
---

ServiceControl as an application can be used to process the entire message load of a system. This article provides general guidelines, recommendations, and performance benchmarks to help determine the resources to provide for a production environment. To identify the hardware specifications for any system environment, a combination of testing with the system and the information provided below will need to be used.

## General recommendations

* Install ServiceControl on a dedicated server in production.
* 6 GB of RAM minimum
* 2 GHz quad core CPU or better
* [Database path](/servicecontrol/creating-config-file.md#host-settings-servicecontroldbpath) located on disks suitable for low latency write operations (fiber, solid state drives, raid 10), with a recommended IOPS of at least 7500.

NOTE: To ensure disk performance, use a benchmark tool, such as [CrystalDiskMark](https://crystalmark.info/en/software/crystaldiskmark/).

### Server performance monitoring

Due to changes in the system it supports, the requirements for a server hosting ServiceControl can change over time. It is highly recommended that monitoring of the CPU, RAM, disk I/O, and network I/O for the server running ServiceControl be included.

Real disk, CPU, RAM, and network performance can be monitored with the Windows Resource Monitor and/or Windows Performance counters.

## Benchmark data

ServiceControl version 3.0.0 was tested to validate performance improvements made between version 2 and version 3. 

NOTE: The test harness used is a simplified test. It is strongly recommended to run performance tests with realistic message loads to validate baseline hardware requirements. This benchmark data is meant only as a point of reference to assist with determining dedicated ServiceControl server requirements.

### Hardware used

* 16 vCPU
* 64 GB RAM
* 2x7500 IOPS striped disk dedicated for the database

### Audit message processing throughput

Message Size | Messages Per Second
---- | ----
13 KB | 140 msgs/s
66 KB | 80 msgs/s

### Disk usage

While disk usage was not captured across all tests, for a scenario using a 66 KB message size and storing 450,000 messages the total database size was 4 GB.

## Suggestions to improve performance

### More RAM

The embedded RavenDB will utilize additional RAM to improve indexing performance.

### Message size / MaxBodySizeToStore

In general, the smaller the message, the quicker ServiceControl will be able to process audit records. Consider [using smaller messages](https://particular.net/blog/putting-your-events-on-a-diet). For larger message payloads, consider using the [DataBus feature](/nservicebus/messaging/databus/).

In addition, for audit messages, lower the [`ServiceControl/MaxBodySizeToStore`](/servicecontrol/creating-config-file.md#performance-tuning-servicecontrolmaxbodysizetostore) setting to skip storage of larger audit messages. This setting will only reduce load if a non-binary [serialization](/nservicebus/serialization/) is used.

WARNING: When using ServiceInsight, the message body will not be viewable for messages that exceed the `ServiceControl/MaxBodySizeToStore` limit.

### Use a dedicated disk for the database

Use a dedicated disk for the ServiceControl [database path](/servicecontrol/creating-config-file.md#host-settings-servicecontroldbpath).

Additionally, it is possible to store the embedded database index files on a separate disk. Use the [`Raven/IndexStoragePath`](/servicecontrol/creating-config-file.md#host-settings-ravenindexstoragepath) setting to change the index storage location.

### Azure disk limitations

Using multiple 7500 IOPS disks in striped mode in Azure may not improve performance due to increased latency; consider [scaling out ServiceControl to multiple instances](#suggestions-to-improve-performance-scale-out).

### Scale out

If it is not possible to scale up ServiceControl to handle system volume, partition audit processing between multiple instances of ServiceControl. See [Multiple ServiceControl Instances](distributed-instances.md) for more details.
