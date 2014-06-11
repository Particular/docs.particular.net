---
title: ServiceControl Capacity and Planning
summary: Details the ServiceControl capacity, throughput, and storage considerations to plan and support production environments
tags:
- ServiceControl
---

ServiceControl is a monitoring tool for production environments. As for other production tools, you must plan and maintain the deployment over time.

The primary job of ServiceControl is to monitor error and audit queues, reading messages flowing into those queues and storing them in its own database. In a production environment, ServiceControl has an impact on the disk space where its data is stored, and its throughput capacity is impacted by the overall system load.

### ServiceControl Storage

ServiceControl stores its data in a RavenDB embedded instance, whose storage location on disk can be [customized](/ServiceControl/configure-ravendb-location). The location of the database has a significant impact on the overall system behavior in terms of performance and throughput. You should store the database in a high-performance storage device that is connected to the SC machine with a high-throughput connection.

The storage size that ServiceControl requires depends on the production load and is directly related to the quantity and size of messages that flow into the system.

Since ServiceControl is intended to be a recent-history storage to support ServicePulse and ServiceInsight monitoring and debugging activity (as opposed to being a long-term data archiving system) it is configured with a [default expiration policy](/ServiceControl/how-purge-expired-data) that deletes old messages after a predefined time. The expiration policy can be [customized](/ServiceControl/how-purge-expired-data) to increase the amount of time data is retained, which impacts the storage requirements of ServiceControl.

To access raw message data for storage in a long-term archive or in a specialized BI database:

* *Query the ServiceControl HTTP API*: This provides a JSON stream of audited messages (headers, body, and context) that can be imported into another database.

* *Activate audit.log queuing in ServiceControl*, which copies the audited messages to the `audit.log` queue where a custom endpoint can handle incoming messages to apply custom logic.
	* This is turned off by default, as opposed to copying failed messages to the `error.log` queue, which is on by default.

**NOTE**

* The maximum supported size of a RavenDB database is 16TB.
* A failed message *never* expires and is retained indefinitely in the ServiceControl database.

### ServiceControl Throughput

ServiceControl is not intended to be a real-time monitoring system. ServiceControl guarantees that messages are audited and stored in its database. ServiceControl makes messages flowing into the system available for a time that is impacted by the current throughput, which in turn depends on the average load of the entire system and the chosen transport.
