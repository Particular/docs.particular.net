---
title: SQL Server Transport deployment options
summary: Describes available SQL Server transport deployment options
reviewed: 2016-03-24
tags:
- SQL Server
- Transports
redirects:
- nservicebus/sqlserver/multiple-databases
---

When using the default configuration, SQL Server Transport assumes that all tables used for storing messages for endpoints are located in a single catalog and within a single schema. The configuration can be changed to partition message storage between different schemas and catalogs. The schemas and catalogs can also be specified at a queue level. For example, the error and the audit queues can be configured to use a different schema and a different database catalog.

The supported deployment options are:

 * **default**: all queues are stored in a single catalog and a single schema.
 * **multi-schema**: queues are stored in a single catalog but in more than one schema. 
 * **multi-instance**: queues are stored in multiple catalogs on more than one SQL Server instance.
 * **multi-catalog**: queues are stored in multiple catalogs but on a single SQL Server instance. This mode is indirectly supported by using *multi-instance* option, and requires using DTC. In this document both options are covered under *multi-instance* term.

NOTE: To properly identify the chosen deployment option all queues that the endpoint interacts with need to be taken into consideration, including error and audit queues. If either of them are stored in a separate SQL Server instance then the deployment is a *multi-instance* one.

The transport will route messages to destination based on the configuration. If no specific configuration has been provided for a particular destination, the transport assumes the destination has the same configuration as the sending endpoint (i.e. identical schema, catalog and instance name). If the destination has a different configuration and it hasn't been provided, then exception will be thrown when sending a message, because the transport wouldn't be able to connect to the destination queue.


## Modes overview


### Default (single schema, single catalog, single SQL Server instance)

 * Has simple configuration and setup.
 * Doesn't require Distributed Transaction Coordinator (DTC).
 * The snapshot (backup) of the entire system state can be done by backing up a single database. It is especially useful if business data is also stored in the same database.
 * Can be monitored with ServiceControl.


### Multi-schema

 * Has simple configuration and setup.
 * Doesn't require DTC.
 * The snapshot (backup) of the entire system state is done by backing up a single database. It is especially useful if business data is also stored in the same database.
 * Enables security configuration on a schema level.
 * Can't be monitored with ServiceControl.


### Multi-instance

WARNING: The *multi-instance* option won't be directly supported in Versions 4 and higher. Instead, a guidance will be provided on how to achieve a similar result using built-in SQL Server features. However, in Versions 4 and higher the *multi-catalog* option will be directly supported.

 * Requires DTC, or using Outbox and storing business data in the same database as Outbox data.
 * Can't be monitored with ServiceControl.


### Multi-instance with store-and-forward

WARNING: This option will not be supported in Versions 4 and above.

SQL Server transport does not support store-and-forward mechanism natively. Therefore, if the receiving endpoint's infrastructure e.g. DTC or SQL Server instance is unavailable especially in a *multi-instance* mode, messages to the endpoint can't be delivered. The sending endpoint and all the other endpoints that depend on it will also be unavailable. The problem can be addressed by using the [Outbox](/nservicebus/outbox/) feature. 

The Outbox feature can be used to avoid escalating transactions to DTC, when each endpoint has a separate database for storing queues and business data on the same SQL Server instance. However, it's not possible to avoid distributed transactions when any of the queues are on a different SQL Server instance or catalog. That means that in order to avoid escalation, each endpoint should have their dedicated error and audit queues.

When using Outbox:

 * Messages are not dispatched immediately after the `Send()` method is called. Instead, they are first stored in the Outbox table in the same database that the endpoint's persistence is using. After the handler logic completes successfully, the messages stored in the Outbox table are forwarded to their final destinations.
 * If any of the forward operations fails, the message sending will be retried using the standard [retry mechanism](/nservicebus/errors/automatic-retries.md). Attempting to retry the forward operation may result in dispatching some messages multiple times. However, the Outbox feature automatically de-duplicates the incoming messages based on their IDs, therefore providing `exactly-once` message delivery. The receiving endpoint also has to be configured to use the Outbox.
