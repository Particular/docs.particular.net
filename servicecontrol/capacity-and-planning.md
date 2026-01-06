---
title: ServiceControl capacity planning
summary: Outlines the ServiceControl capacity, throughput, and storage considerations for planning and supporting production environments
reviewed: 2024-12-02
isLearningPath: true
---

ServiceControl is a monitoring tool for production environments. As with other production monitoring tools, it is important to plan its deployment to ensure it remains performant over time.

The primary job of ServiceControl is to collect information on system behavior in production. It does so by collecting error, audit, and health messages from dedicated queues. ServiceControl reads messages flowing into those queues and stores them in its embedded database. In a production environment (and to a lesser degree in development, staging, and testing environments), ServiceControl may have significant impact on the performance and capacity of the disk where its data is stored, and its throughput capacity must be considered with regard to the overall system load and throughput.

## Storage

### Location

Each ServiceControl instance stores its data in a [RavenDB](https://ravendb.net) instance. 

- For ServiceControl instances deployed via PowerShell or the ServiceControl Management Utility the database is run via an [embedded RavenDB server](/servicecontrol/configure-ravendb-location.md).
- For ServiceControl instances deployed via Containers the database is run via a [dedicated container](/servicecontrol/ravendb/containers.md).

The location of the database has a significant impact on overall system performance and throughput. The database files should be located on a high-performance storage device with a high-throughput connection to the machine hosting ServiceControl.

### Size

The storage size that ServiceControl requires depends on the production load and is directly related to the quantity and size of messages that flow into the system.

ServiceControl provides "recent-history" storage to support ServicePulse monitoring and debugging. This is different to a data archiving system that is intended to provide long-term archiving and storage (measured in years, subject to various business or regulatory requirements).

ServiceControl is configured with default expiration policies that delete old messages after predefined time periods. The expiration policies may be customized to decrease or increase the amount of time that data is retained, which impacts the storage requirements of ServiceControl.

To limit the rate at which the database grows, the body of an audit messages may be truncated if it exceeds a configurable threshold.

See also: [Automatic Expiration of ServiceControl Data](how-purge-expired-data.md).

**NOTE**

 * The maximum supported size of a RavenDB embedded database is 16 TB.
 * Failed messages *never* expire and are retained indefinitely in the ServiceControl database.

### Performance

From a performance perspective, ServiceControl is similar to a database installation. It requires a significant amount of disk and network I/O due to process audit, error and monitoring messages. Each of these message-processing operations requires disk I/O. The higher the message throughput of an environment, the higher the required disk I/O.

For this reason, it is best to store ServiceControl data on a disk with low latency for I/O operations. Indexes are continuously updated and keeping them in memory requires significant RAM. Indexes that cannot be stored fully in RAM are more likely to be stale. If full-text indexing is enabled, messages are added to full-text search, which requires the CPU to have sufficient capacity for updating indexes. Full-text search for ServiceControl error or audit instances can be configured in the ServiceControl Management Utility.

For more details, see [Hardware Considerations](servicecontrol-instances/hardware.md).

## Accessing data and audited messages

### Forwarding queues

ServiceControl may be configured to forward consumed messages to other queues for further consumption by third party systems.

See also: [Forwarding Queues](errorlog-auditlog-behavior.md).

### HTTP API

The ServiceControl HTTP API provides a JSON stream of audited and error messages (headers, body, and context) that can be imported into another database.

> [!NOTE]
> The ServiceControl HTTP API is subject to changes and enhancements that may not be backward compatible. Use of the HTTP API by third parties is discouraged at this time.

## Throughput

ServiceControl consumes audit, error, and control messages from all endpoints configured to forward those messages to it. This means the throughput (measured in received and processed messages per second) required by ServiceControl is the aggregate throughput of all endpoints forwarding messages to its queues.

The throughput of ServiceControl depends on multiple factors. Message size and network bandwidth have a significant effect. Another factor is the transport type used by the system.

### Transport type

Different transport types have different throughput capabilities.

The transports supported by ServiceControl provide varying levels of throughput. MSMQ and SQL Server typically provide the highest.

The throughput provided by Azure Storage Queues and Azure Service Bus varies significantly depending on which deployment options are chosen and various other factors inherent to cloud deployment scenarios.

It is recommended to perform realistic throughput tests on ServiceControl using the chosen transport, and using deployment options that are as close as possible to those planned for production.
