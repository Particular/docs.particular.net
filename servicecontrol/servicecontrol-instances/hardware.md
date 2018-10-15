---
title: ServiceControl Hardware Considerations
summary: Describes performance benchmarks performed for ServiceControl
reviewed: 2018-10-15
---

If ServiceControl is installed on a virtual machine, ensure the machine is capable of high levels of network and disk I/O traffic. The amount of I/O required depends on the system being monitored, the number of messages being processed, and the transport being used. 

## General Recommendations

* Use dedicated hardware for ServiceControl in production.
* 6GB of RAM minimum
* 2Ghz quad core CPU or better
* [Database Path](/servicecontrol/creating-config-file.md#host-settings-servicecontroldbpath) is located on disks suitable for low latency write operations (fiber, solid state drives, raid 10), with a recommended IOPS of at least 7500.

### Monitoring

CPU, RAM, disk IO, and Network IO for the server running ServiceControl should be monitored.

## Benchmark Data

Version 3.x of ServiceControl was tested using a test harness to validate performance improvements made between v2 and v3. 

NOTE: The test harness is a highly simplified test, it is highly recommended to run performance tests with realistic message loads to validate baseline hardware requirements. This data is only meant as a point of reference to help size a dedicated ServiceControl server.

### Hardware Used

* 16 vCPU
* 64 GB RAM
* 2x7500 IOPS striped disk dedicated for the database

### Performance by message size

Message Size | Messages Per Second
---- | ----
13 Kb | 140 msgs/s
66 Kb | 80 msgs/s

### Disk Usage

66 KB , 4 GB database, 450K messages

## Suggestions to improve performance

### More RAM

The embedded RavenDB will utilize additional RAM to improve indexing performance.

### Message Size / MaxBodySizeToStore

In general, the smaller the message the quicker ServiceControl will be able to process audit records. Consider [putting your events on a diet](https://particular.net/blog/putting-your-events-on-a-diet).

In addition, for audit messages, lower the `[ServiceControl/MaxBodySizeToStore](/servicecontrol/creating-config-file.md#performance-tuning-servicecontrolmaxbodysizetostore)` setting to skip storage of larger audit messages. This setting will only help if the [serialization](/nservicebus/serialization/) being used is non-binary.

NOTE: When using ServiceInsight the message body will not be viewable for messages that exceed the `ServiceControl/MaxBodySizeToStore` limit.

### Use a dedicated disk for the database

Use a dedicated disk for the ServiceControl [database path](/servicecontrol/creating-config-file.md#host-settings-servicecontroldbpath).

Additionally it is possible to store the embedded database index files on a separate disk. Use the `[Raven/IndexStoragePath](/servicecontrol/creating-config-file.md#host-settings-ravenindexstoragepath)` to place the RavenDB indexes on a different disk.
