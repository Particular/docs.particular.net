---
title: ServiceControl Capacity Planning
summary: Outlines the ServiceControl capacity, throughput, and storage considerations to plan and support production environments
reviewed: 2018-06-21
---

ServiceControl is a monitoring tool for production environments. As with other production monitoring tools, it is important to plan out its deployment to ensure if remains performant over time.

The primary job of ServiceControl is to collect information on system behavior in production. It does so by collecting error, audit, and health messages from dedicated queues. ServiceControl reads messages flowing into those queues and stores them in its embedded database. In a production environment (and to a lesser degree in development, staging, and testing environments), ServiceControl has an impact on the disk space where its data is stored, and its throughput capacity must be considered with regard to the overall system load and throughput.


### Storage


#### Location

Each ServiceControl instance stores its data in a [RavenDB embedded](https://ravendb.net/docs/search/3.0/csharp?searchTerm=RavenDB%20embedded) instance. The location of the database has a significant impact on the overall system behavior in terms of performance and throughput. Configure the embedded database files in a high-performance storage device that is connected to the ServiceControl machine with a high-throughput connection.


#### Size

The storage size that ServiceControl requires depends on the production load and is directly related to the quantity and size of messages that flow into the system.

ServiceControl is intended to be a "recent-history storage" to support ServicePulse and ServiceInsight monitoring and debugging activity. This is different from a data archiving system that is intended to provide long-term archiving and storage solutions (measured in years, subject to various business or regulatory requirements).

ServiceControl is configured with a default expiration policy that deletes old messages after a predefined time. The expiration policies can be customized to decrease or increase the amount of time data is retained, which impacts the storage requirements of ServiceControl.

To limit the rate at which the database grows, the body of an audit messages can be truncated if it exceeds a configurable threshold.

See also: [Automatic Expiration of ServiceControl Data](how-purge-expired-data.md).

**NOTE**

 * The maximum supported size of the RavenDB embedded database is 16TB.
 * Failed messages *never* expire and are retained indefinitely in the ServiceControl database.


#### Performance

From a performance perspective, ServiceControl is similar to a database installation. It requires a significant amount of disk and network I/O due to processing of audit, error and monitoring messages. Each of these message-processing operations requires disk I/O. The higher the message throughput of an environment, the higher the required disk I/O.

For this reason, it is necessary to store ServiceControl data on a disk with the lowest possible latency for I/O operations. Indexes require continuous updating and will require significant RAM to allow those indexes to be kept in memory. Processing of indexes that cannot be stored fully in RAM will result in a higher likelihood of those indexes being stale. Since messages are added to a full-text search, it is also necessary to make sure the CPU will not become a bottleneck in updating indexes.

For more details, see [Hardware Considerations](servicecontrol-instances/hardware.md).

### Accessing data and audited messages


#### Alternate audit and error queues

ServiceControl can be configured to forward any consumed messages into alternate queues. A copy of any message consumed by ServiceControl is available from these alternate queues.

See also: [Forwarding Queues](errorlog-auditlog-behavior.md).


#### Query the ServiceControl HTTP API

This provides a JSON stream of audited and error messages (headers, body, and context) that can be imported into another database.

NOTE: ServiceControl HTTP API is subject to changes and enhancements that may not be backward compatible. Use of this HTTP API is discouraged by third parties at this time.


### Throughput

ServiceControl consumes audited, error, and control messages in its database. It does so for all endpoints that are configured to forward these messages to the queues monitored by ServiceControl. This means that the throughput (measured in received and processed messages per second) required by ServiceControl is the aggregate throughput of all endpoints forwarding messages to its queues.

The throughput of ServiceControl depends on multiple factors. Message size and network bandwidth have a significant effect on throughput. Another factor is the transport type used by the system.


#### Transport type

Different transports provide different throughput capabilities.

The transports supported by ServiceControl out-of-the-box (i.e. MSMQ, RabbitMQ, SQL Server, Azure Storage Queues, and Azure Service Bus) provide varying throughput numbers, with MSMQ and SQL Server providing the highest throughput numbers.

Azure Storage Queues and Azure Service Bus throughput varies significantly based on deployment options and multiple variables inherent to cloud deployment scenarios.

It is recommended to plan and perform realistic throughput tests on ServiceControl using the transport of choice and deployment options that are as close as possible to the planned production deployment. For additional questions or information [contact support](https://particular.net/contactus).
