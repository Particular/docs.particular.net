---
title: ServiceControl capacity and planning
summary: Details the ServiceControl capacity, throughput and storage considerations to plan and support production environments
tags:
- ServiceControl
---

ServiceControl is intended to be a monitoring tool for production environments and, as each production tool, the deployment must be planned and mantained over time.

The primary job of ServiceControl is to monitor error and audit queues reading messages flowing into those queues and storing them in its own database. In a production environment ServiceControl have an impact on the disk space, where its data are stored, and is impacted in its throughput capacity by the overall system load.

### ServiceControl storage

ServiceControl stores its data in a RavenDB Embedded instance, whose storage location on disk can be [customized](/ServiceControl/configure-ravendb-location). The location of the database has a significant impact on the overall system behavior in terms of performance and throughput. It is recommended that the database is stored in a high-performance storage device that is connected to the SC machine with a high-throughput connection.

The storage size that ServiceControl requires depends on the production load and is directly related to the amount and size of messages that flow into the system.

Since ServiceControl is intended to be a recent-history storage to support ServicePulse and ServiceInsight monitoring and debugging activity (as opposed to being a long-term data archiving system) it is setup with a [default expiration policy](/ServiceControl/how-purge-expired-data) that deletes old messages after a predefined amount of time.

The expiration policy can be [customized](/ServiceControl/how-purge-expired-data) to increase the amount of time data is retained impacting on the storage requirements of ServiceControl.

If the requirement is to gain access to raw message data in order to store it in long term archiving storage or in a specialized BI database, it can be easily done:

* *by querying the ServiceControl HTTP API*: This will provide a JSON stream of audited messages (headers, body and context) that can be imported into another database for various purposes;

* *by activating audit.log queuing in ServiceControl* which copies the audited messages to the `audit.log` queue where a custom endpoint can handle incoming messages in order to apply custom logic;
	* this is turned off by default, as opposed to copying failed messages to `error.log` queue which is on by default

**NOTE**

* The maximum supported size of a RavenDB database is 16TB.
* Failed message *never* expires and will be retained indefinitely in the ServiceControl database;

### ServiceControl throughput

ServiceControl is not intended to be a real time monitoring system, ServiceControl guarantees that messages will be audited and stored in the its database. The delay after a message that flew into the system is available in ServiceControl is impacted by the current throughput that depends on the avarage load of the entire system; the throughput may also vary depending on the chosen transport.
