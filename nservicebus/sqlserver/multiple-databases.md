---
title: SQL Server transport setup - default, multi-schema, multi-catalog and multi-instance
summary: How to configure and manage SQL Server transport in default, multi-schema, multi-catalog and multi-instance modes
tags:
- SQL Server
- Transports
---

In the default SQL Server transport setup, the messages and other data are stored in multiple tables in a single database. The default schema is `dbo`, but it is possible to [specify a custom schema](/nservicebus/sqlserver/configuration.md#custom-database-schemas).

In some environments it might be necessary to use a more advanced setup. The supported options are:
 * ***default***: single schemas in a single database (Versions 1.x and higher)
 * ***multi-schema***: multiple schemas in a single database (Versions 2.1 and higher)
 * ***multi-catalog***: multiple databases in a single SQL Server instance (Versions 2.1 and higher)
 * ***multi-instance***: multiple databases in various SQL Server instances (Versions 2.1 to 2.x).

The transport will route messages to destination endpoints based on the configuration. If no specific configuration has been provided for a particular destination endpoint, the transport assumes the destination has the same configuration (schema, database and instance name) as the sending endpoint. If the destination endpoint has a different configuration and it hasn't been provided, then exception will be thrown immediately (since the transport cannot connect to the destination queue).

WARNING: SQL Server transport does not support store-and-forward mechanism on a transport level, so there is no dead-letter queue. It means that in *multi-catalog* and *multi-database* modes if a remote endpoint's infrastructure (e.g. DTC or SQL Server instance) is down, then messages can't be delivered to it. That makes this particular endpoint, and all endpoints depending on it, unavailable. That problem can be addressed by using [Outbox](/nservicebus/outbox/) feature, as explained in [*Multi-catalog* and *multi-instance* with store-and-forward](#multi-catalog-and-multi-instance-with-store-and-forward) section. 

NOTE: Sending messages between endpoints in *multi-catalog* and *multi-instance* modes causes the transaction to escalate to a distributed transaction. In the *multi-catalog* mode the escalation might be avoided if [Outbox](/nservicebus/outbox/) feature is turned on, and Outbox data and business data are stored in the same database. Storing Outbox data in the same database as business data in practice means choosing NHibernate persistence, and configuring it to use the appropriate database.

# Modes overview

## Default

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
- Can be monitored with ServiceControl

## Multi-catalog

- Supported in Versions 2.1 and higher
- Requires Distributed Transaction Coordinator by default
- Escalation to distributed transactions can be avoided by using Outbox and storing business data in the same database as Outbox data
- Can be monitored with ServiceControl

## Multi-instance

- Supported in Versions 2.1 to 3.x, won't be supported in Versions 4.0 and higher
- Requires Distributed Transaction Coordinator, even when using Outbox
- Can't be monitored with ServiceControl

## Multi-catalog and multi-instance with store-and-forward

In order to overcome the limitation caused by the lack of store-and-forward mechanism on a transport level, a higher level store-and-forward mechanism needs to be used. That functionality is essentially provided by [Outbox](/nservicebus/outbox/) feature, because when Outbox is enabled:
 * Each endpoint has its own database in which it stores both the queues and the user data
 * Messages are not dispatched immediately after the `Send()` method is called. Instead they are first stored in the Outbox table in the same database that endpoint's persistence is using. After the handler logic completes successfully, the messages stored in the Outbox table are forwarded to their destinations.
 * If any of the forward operations fails, the message sending will be retried using the standard [retry mechanism](/nservicebus/errors/automatic-retries.md). This might result in sending some messages multiple times, which is known as `at-least-once` delivery. The Outbox feature performs automatically de-duplication of incoming messages based on their IDs, effectively providing `exactly-once` message delivery.


=====> OLD => move to configuration.md <=========
## Single database

Typically when using SQL Server transport, the endpoints are set up to use the same database for the storing the queues. Sending a message involves executing a SQL statement that results in delivering the message directly to the destination queue. The message is delivered directly without a store-and-forward mechanism. 

Using a single database doesn't require Distributed Transaction Coordinator (MS DTC). Another advantage is the ability to take a snapshot of entire system state (all the queues) by backing up a database. This is most useful when the business data is also stored in the same database.

## Single database with multiple schemas

The default schema used by SQL Server transport is `dbo`. To specify a different schema use the following API:

snippet:sqlserver-singledb-multischema

or using a configuration file:

snippet:sqlserver-singledb-multischema-config

If two endpoints use different schemas then additional configuration is required. The sender needs to know the schema of the receiver, and subscriber needs the schema of the publisher. 

The schema for another endpoint can be specified in the following ways:

snippet:sqlserver-singledb-multidb-push

or:

snippet:sqlserver-singledb-multidb-pull

NOTE: Even if two endpoints use different schemas the SQL Server transport will assume they use the same database. The different connection string must be passed explicitly, as described in the following section.

## Multiple databases

Endpoints can also use separate databases. That scenario requires DTC. 

NOTE: Due to the lack of store-and-forward mechanism, if a remote endpoint's database or DTC infrastructure is down, the endpoint cannot send messages to it. This potentially renders the endpoint unavailable (and also all other endpoints depending on it directly or indirectly).


## Current endpoint

SQL Server transport defaults to `dbo` schema and uses `NServiceBus/Transport` connection string from the configuration file to connect to the database. The default schema can be changed using following API

snippet:sqlserver-multidb-current-endpoint-schema

or by providing additional `Queue Schema` parameter in the connection string

snippet:sqlserver-multidb-current-endpoint-schema-config

The second approach has precedence over the first one.

The other parameters (database and instance name/address) can be changed in code using the connection string API

snippet:sqlserver-multidb-current-endpoint-connection-string

NOTE: `Queue Schema` parameter can also be used in the connection string provided via code.

NOTE: Starting with `V1.2.3` of the `SQL Server Transport` the `Queue Schema` parameter is supported only when used used in the connection string provided via code or via configuration.

NOTE: Unlike in the SQL Server transport, the connection string configuration API in NServiceBus core favors code over config which means then configured both in `app.config` and via the `ConnectionString()` method, the latter will win.


## Other endpoints

If a particular remote endpoint requires customization of any part of the routing (schema, database or instance name/address), appropriate values have to be provided either via code or via configuration convention.


### Push mode

In the push mode the whole collection of endpoint connection information objects is passed during configuration time.

snippet:sqlserver-multidb-other-endpoint-connection-push


### Pull mode

The pull mode can be used when specific information is not available at configuration time. One can pass a `Func<String, ConnectionInfo>` that will be used by the SQL Server transport to resolve connection information at runtime.

snippet: sqlserver-multidb-other-endpoint-connection-pull


### Configuration

Endpoint-specific connection information is discovered by reading the connection strings from the configuration file with `NServiceBus/Transport/{name of the endpoint in the message mappings}` naming convention. If such a connection string is found, it is used for a given endpoint and this setting has precedence over the code-provided connection information.

Given the following mappings:

snippet:sqlserver-multidb-messagemapping

and the following connection strings:

snippet:sqlserver-multidb-connectionstrings

the messages sent to `billing` will go to database `Billing` on server `DbServerB` while the messages to `sales` will go to the database and server set by default i.e. `MyDefaultDB` on server `DbServerA`.


### ServiceControl

ServiceControl requires the `error` and `audit` queues to be present in a single database. In a system where there are multiple databases, used by various endpoints, this can be achieved by redirecting the queues using the following configuration endpoint configuration:

snippet:sqlserver-multidb-redirect-audit-error

For ServiceControl to retrying a message, after processing failure, it needs to have the same endpoint to connection string mappings as user endpoints e.g.

snippet:sqlserver-multidb-sc

Users need to make sure the connection string mapping is kept in sync between all the endpoints and ServiceControl.