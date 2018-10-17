---
title: ServiceControl Hardware Considerations
summary: Hardware recommendations for running ServiceControl
reviewed: 2018-10-17
---

ServiceControl as an application can be used to process the entire message load of a system. This articles provides general guidelines, recommendations, and performance benchmarks to help determine the resources to provide for a production environment. To identify the hardware specifications for any system environment a combination of testing with the system and the information provided below will need to be used.

## General Recommendations

* Use dedicated hardware for ServiceControl in production.
* 6GB of RAM minimum
* 2Ghz quad core CPU or better
* [Database Path](/servicecontrol/creating-config-file.md#host-settings-servicecontroldbpath) is located on disks suitable for low latency write operations (fiber, solid state drives, raid 10), with a recommended IOPS of at least 7500.

NOTE: To ensure disk performance use a benchmark tool, for example [CrystalDiskMark](http://crystalmark.info/software/CrystalDiskMark/index-e.html).

### Server Performance Monitoring

Due to changes in the system it supports the requirements for a server hosting ServiceControl can change over time. It is highly recommended that monitoring of the CPU, RAM, disk IO, and network IO for the server running ServiceControl be included.

Real disk, CPU, RAM, and network performance can be monitored with the Windows Resource Monitor and/or Windows Performance counters.

## Benchmark Data

Version 3.0.0 of ServiceControl was tested to validate performance improvements made between version 2 and version 3. 

NOTE: The test harness used is a highly simplified test. It is highly recommended to run performance tests with realistic message loads to validate baseline hardware requirements. This benchmark data is only meant as a point of reference to assist with determining dedicated ServiceControl server requirements.

### Hardware Used

* 16 vCPU
* 64 GB RAM
* 2x7500 IOPS striped disk dedicated for the database

### Performance by message size

Message Size | Messages Per Second
---- | ----
13 KB | 140 msgs/s
66 KB | 80 msgs/s

### Disk Usage

While we did not capture disk usage across all tests, for a scenario using a 66 KB message size and storing 450,000 messages the total database size was 4 GB.

## Suggestions to improve performance

### More RAM

The embedded RavenDB will utilize additional RAM to improve indexing performance.

### Message Size / MaxBodySizeToStore

In general, the smaller the message the quicker ServiceControl will be able to process audit records. Consider [putting your events on a diet](https://particular.net/blog/putting-your-events-on-a-diet). For larger message payloads consider using the [DataBus feature](/nservicebus/messaging/databus/).

In addition, for audit messages, lower the `[ServiceControl/MaxBodySizeToStore](/servicecontrol/creating-config-file.md#performance-tuning-servicecontrolmaxbodysizetostore)` setting to skip storage of larger audit messages. This setting will only reduce load if the [serialization](/nservicebus/serialization/) used is non-binary.

WARNING: When using ServiceInsight the message body will not be viewable for messages that exceed the `ServiceControl/MaxBodySizeToStore` limit.

### Use a dedicated disk for the database

Use a dedicated disk for the ServiceControl [database path](/servicecontrol/creating-config-file.md#host-settings-servicecontroldbpath).

Additionally, it is possible to store the embedded database index files on a separate disk. Use the `[Raven/IndexStoragePath](/servicecontrol/creating-config-file.md#host-settings-ravenindexstoragepath)` setting change the index storage location.
