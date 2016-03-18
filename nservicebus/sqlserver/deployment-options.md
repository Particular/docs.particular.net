---
title: SQL Server transport deployment options
summary: Describes available SQL Server transport deployment options
tags:
- SQL Server
- Transports
redirects:
- nservicebus/sqlserver/multiple-databases
---

In the default SQL Server transport setup, all queues are stored in tables located in a single catalog and use a single schema. The default schema is `dbo`, but it is possible to [specify a custom schema](/nservicebus/sqlserver/configuration.md#custom-database-schemas). In some environments it might be necessary to use a more advanced setup. 

The supported deployment options are:
 * ***default***: single schema in a single database (Versions 1.x and higher)
 * ***multi-schema***: multiple schemas in a single database (Versions 2.1 and higher)
 * ***multi-catalog***: multiple catalogs in a single SQL Server instance (Versions 2.1 to 2.x)
 * ***multi-instance***: multiple databases in various SQL Server instances (Versions 2.1 to 2.x).

NOTE: *Multi-instance* is relevant also for error and audit queues, meaning that if either of them is stored in a separate SQL Server instance then the deployment is a *multi-instance* one.

The transport will route messages to destination endpoints based on the configuration. If no specific configuration has been provided for a particular destination endpoint, the transport assumes the destination has the same configuration as the sending endpoint (i.e. identical schema, database and instance name). If the destination endpoint has a different configuration and it hasn't been provided, then exception will be thrown immediately (since the transport cannot connect to the destination queue).

WARNING: SQL Server transport does not support store-and-forward mechanism on a transport level. It means that in *multi-catalog* and *multi-database* modes if a remote endpoint's infrastructure (e.g. DTC or SQL Server instance) is down, then messages can't be delivered to it. That makes this particular endpoint, and all endpoints depending on it, unavailable. That problem can be addressed by using [Outbox](/nservicebus/outbox/) feature, as explained in [*Multi-catalog* and *multi-instance* with store-and-forward](#multi-catalog-and-multi-instance-with-store-and-forward) section. 

NOTE: Sending messages between endpoints in *multi-catalog* and *multi-instance* modes causes the transaction to escalate to a distributed transaction. The escalation might be avoided if [Outbox](/nservicebus/outbox/) feature is turned on, and Outbox data and business data use the same connection string. Note that simply storing data in the same database might not be sufficient condition as described in [Entity Framework caveats](/nservicebus/sqlserver/configuration.md#entity-framework-caveats) section, because the connection string must be identical.

# Modes overview

## Default (single schema, single catalog, single SQL Server instance)

- Supported in Versions 1.0 and higher
- Simple configuration and setup
- Doesn't require Distributed Transaction Coordinator (MS DTC)
- The snapshot (backup) of the entire system state is done by backing up a single database (especially useful if business data is also stored in the same database)
- Can be monitored with ServiceControl

## Multi-schema

- Supported in Versions 2.1 and higher
- Simple configuration and setup
- Doesn't require Distributed Transaction Coordinator (MS DTC)
- The snapshot (backup) of the entire system state is done by backing up a single database (especially useful if business data is also stored in the same database)
- A fine-grained security control on a database schema level
- Can't be monitored with ServiceControl

## Multi-catalog

- Supported in Versions 2.1 to 2.x, legacy support in Versions 3.x, won't be supported in Versions 4.0 and higher
- Requires Distributed Transaction Coordinator (MS DTC), or using Outbox and storing business data in the same database as Outbox data
- Can't be monitored with ServiceControl

## Multi-instance

- Supported in Versions 2.1 to 2.x, legacy support in Versions 3.x, won't be supported in Versions 4.0 and higher
- Requires Distributed Transaction Coordinator, or using Outbox and storing business data in the same database as Outbox data
- Can't be monitored with ServiceControl

## Multi-catalog and multi-instance with store-and-forward

In order to overcome the limitation caused by the lack of store-and-forward mechanism on a transport level, a higher level store-and-forward mechanism needs to be used. That functionality is essentially provided by [Outbox](/nservicebus/outbox/) feature, because when Outbox is enabled:
 * Each endpoint has its own database in which it stores both the queues and the user data
 * Messages are not dispatched immediately after the `Send()` method is called. Instead they are first stored in the Outbox table in the same database that endpoint's persistence is using. After the handler logic completes successfully, the messages stored in the Outbox table are forwarded to their destinations.
 * If any of the forward operations fails, the message sending will be retried using the standard [retry mechanism](/nservicebus/errors/automatic-retries.md). This might result in sending some messages multiple times, which is known as `at-least-once` delivery. The Outbox feature performs automatically de-duplication of incoming messages based on their IDs, effectively providing `exactly-once` message delivery.

### ServiceControl

ServiceControl requires the `error` and `audit` queues to be present in a single database. In a system where there are multiple databases, used by various endpoints, this can be achieved by redirecting the queues using the following configuration endpoint configuration:

snippet:sqlserver-multidb-redirect-audit-error

For ServiceControl to retrying a message, after processing failure, it needs to have the same endpoint to connection string mappings as user endpoints e.g.

snippet:sqlserver-multidb-sc

Users need to make sure the connection string mapping is kept in sync between all the endpoints and ServiceControl.