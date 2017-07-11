---
title: Connection Settings
reviewed: 2016-09-07
component: SqlTransport
redirects:
- nservicebus/sqlserver/connection-settings
- transports/sqlserver/connection-settings
---

## Using connection pool

The SQL Server transport is built on top of [ADO.NET](https://docs.microsoft.com/en-us/dotnet/framework/data/adonet/index) and will use connection pooling. This may result in the connection pool being shared by the transport, as well as other parts of the endpoint process and the business logic. 

In scenarios where the concurrent message processing limit is changed, or the database connection is used for other purposes as mentioned above, it is advisable to change the connection pool size to ensure it will not be exhausted. See also [SQL Server Connection Pooling and Configuration](https://docs.microsoft.com/en-us/dotnet/framework/data/adonet/sql-server-connection-pooling).

NOTE: In NServiceBus Versions 6 and above the default level of concurrency is based on the number of logical processors and as such, if the maximum and minimum pool sizes are not explicitly set on the connection string a warning message will be logged. See also [Tuning endpoint message processing](/nservicebus/operations/tuning.md) 


## Connection configuration

Connection string can be configured in several ways:

partial: connection-string


## Multiple connection strings

partial: multi-instance


## Custom database schemas

SQL Server transport uses `dbo` as a default schema. Default schema is used for every queue if no other schema is explicitly provided in transport address. That includes all local queues, error, audit and remote queues of other endpoints.

partial: custom-schema

partial: factory


## Circuit Breaker

The SQL transport has a built in circuit breaker to handle intermittent SQL Server connectivity problems.

partial: circuit-breaker
