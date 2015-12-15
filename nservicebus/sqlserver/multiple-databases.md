---
title: Multi-database support
summary: How to configure SQL Server transport to use multiple instances of the database and route messages between them.
tags:
- SQL Server
- Transports
---

The SQL Server transport allows you to select, on per-endpoint basis, where the table queues should be created. The selection can be done on multiple levels:

 * different schemas in a single database
 * different databases in a single SQL Server instance
 * different SQL Server instances

The transport will route messages to destination endpoints based on the configuration. If no specific configuration has been provided for a particular destination endpoint, the transport assumes the destination has the same configuration (schema, database and instance name/address) as the sending endpoint. If this assumption turns out to be false (the transport cannot connect to destination queue), an exception is thrown immediately. There is no store-and-forward mechanism on the transport level (and hence -- no dead-letter queue).

NOTE: If the destination endpoint uses different database or server instance, sending a message to it might cause the transaction to escalate to a distributed transaction. Usually it is not a desired effect and one can use NServiceBus Outbox to avoid it.


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

NOTE: Unlike in the SQL Server transport, the connection string configuration API in NServiceBus core favors code over config which means that if you configure connection string both in `app.config` and via the `ConnectionString()` method, the latter will win.


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
