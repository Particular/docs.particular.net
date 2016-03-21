---
title: SQL Server transport deployment options
summary: Describes available SQL Server transport deployment options
tags:
- SQL Server
- Transports
redirects:
- nservicebus/sqlserver/multiple-databases
---

In default configuration SQL Server transport assumes that all tables used for storing messages are located in a single catalog and within a single schema. The configuration can be changed to partition message storage between different schemas and catalogs. The decision can me made on per queue basis (including error and audit queues).

The supported deployment options are:
 * ***default***: all queues are stored in a single catalog and a single schema (Versions 1.x and higher)
 * ***multi-schema***: queues are stored in a single catalog but in more than one schema (Versions 2.1 and higher)
 * ***multi-catalog***: queues are stored in multiple catalogs but on a single SQL Server instance. This mode is not explicitly supported by SQL Server transport, but can be achieved  by using multi-instance option. Therefore in this document both options are covered under *multi-instance* term.
 * ***multi-instance***: queues are stored in multiple catalogs on more than one SQL Server instance (Versions 2.1 to 2.x).

NOTE: To properly identify the chosen deployment option all queues that endpoint interacts with need to be taken into consideration including error and audit queues. If either of them is stored in a separate SQL Server instance then the deployment is a *multi-instance* one.

The transport will route messages to destination based on the configuration. If no specific configuration has been provided for a particular destination, the transport assumes the destination has the same configuration as the sending endpoint (i.e. identical schema, catalog and instance name). If the destination has a different configuration and it hasn't been provided, then exception will be thrown immediately, because the transport wouldn't be able to connect to the destination queue.

# Modes overview

## Default (single schema, single catalog, single SQL Server instance)

- Supported in Versions 1.0 and higher.
- Has simple configuration and setup.
- Doesn't require Distributed Transaction Coordinator (MS DTC).
- The snapshot (backup) of the entire system state can be done by backing up a single database. It is especially useful if business data is also stored in the same database.
- Can be monitored with ServiceControl.

## Multi-schema

- Supported in Versions 2.1 and higher.
- Has simple configuration and setup.
- Doesn't require Distributed Transaction Coordinator (MS DTC).
- The snapshot (backup) of the entire system state is done by backing up a single database. It is especially useful if business data is also stored in the same database.
- Enables security configuration on a schema level.
- Can't be monitored with ServiceControl.

## Multi-instance (same applies for multi-catalog)

- Supported in Versions 2.1 to 2.x, legacy support is offered in Versions 3.x, won't be supported directly in Versions 4.0 and higher.
- Requires Distributed Transaction Coordinator, or using Outbox and storing business data in the same database as Outbox data.
- Can't be monitored with ServiceControl.

## Multi-instance with store-and-forward

SQL Server transport does not support store-and-forward mechanism natively. It means that in *multi-instance* mode if a remote endpoint's infrastructure (e.g. DTC or SQL Server instance) is down, then messages can't be delivered. That makes this particular endpoint, and all endpoints depending on it, unavailable. 

The problem can be addressed using [Outbox](/nservicebus/outbox/) feature. In addition Outbox can be used to avoid escalating transactions to DTC, given that each endpoint has its own database in which it stores input queue, error and audit queues, and the user data. That means it's not possible to avoid distributed transactions in case when any of the queues (including error and audit) is on a different SQL Server instance or catalog. 

When using Outbox:
 * Messages are not dispatched immediately after the `Send()` method is called. Instead they are first stored in the Outbox table in the same database that endpoint's persistence is using. After the handler logic completes successfully, the messages stored in the Outbox table are forwarded to their final destinations.
 * If any of the forward operations fails, the message sending will be retried using the standard [retry mechanism](/nservicebus/errors/automatic-retries.md). This might result in sending some messages multiple times, which is known as `at-least-once` delivery. The Outbox feature performs automatically de-duplication of incoming messages based on their IDs, effectively providing `exactly-once` message delivery.

As mentioned before store-and-forward requries `error` and `audit` queues per endpoint. As a result system using such a deployment scenario cannot be monitored by a single ServiceControl instance.