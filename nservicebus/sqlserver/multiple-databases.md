---
title: Multi-database support
summary: How to configure SQL Server transport to use multiple instances of the database and route messages between them.
tags:
- SQL Server
- Transports
---

The SQL Server transport supports selecting, on per-endpoint basis, where the table queues should be created. The selection can be done on multiple levels:

 * different schemas in a single database
 * different databases in a single SQL Server instance
 * different SQL Server instances

The transport will route messages to destination endpoints based on the configuration. If no specific configuration has been provided for a particular destination endpoint, the transport assumes the destination has the same configuration (schema, database and instance name/address) as the sending endpoint. If this assumption turns out to be false (the transport cannot connect to destination queue), an exception is thrown immediately. There is no store-and-forward mechanism on the transport level (and hence -- no dead-letter queue).

NOTE: If the destination endpoint uses a different database or server instance, sending a message to it might cause the transaction to escalate to a distributed transaction which may not be desirable. Using the [Outbox](/nservicebus/outbox/ feature, DTC escalations can be avoided as long as the both the endpoint's Outbox and business data share the same database.

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

## Multiple databases with store-and-forward

In order to overcome this limitation a higher level store-and-forward mechanism needs to be used. The [Outbox](/nservicebus/outbox/) feature can be used to effectively implement a distributed decoupled architecture where:
 * Each endpoint has its own database where it stores both the queues and the user data
 * When calling Bus.Send(), messages are stored in the Outbox table rather than getting dispatched immediately. The Outbox table is a database table that resides on the same database as that of the endpoint. After successful execution of the message handler logic, the messages stored in the Outbox table are forwarded to their destinations.
 * Should any of the forward operations were to fail, it will be retried using the standard [retry mechanism](/nservicebus/errors/automatic-retries.md). However, this might result in some messages to be sent multiple times. To mitigate this and to provide `exactly-once` message delivery guarantee, the Outbox feature automatically handles the de-duplication of incoming messages based on their ID.

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