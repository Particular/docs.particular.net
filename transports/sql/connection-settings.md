---
title: Connection Settings
reviewed: 2017-08-23
component: SqlTransport
redirects:
- nservicebus/sqlserver/connection-settings
- transports/sqlserver/connection-settings
---

## Using connection pool

The SQL Server transport is built on top of [ADO.NET](https://docs.microsoft.com/en-us/dotnet/framework/data/adonet/index) and will use connection pooling. This may result in the connection pool being shared by the transport, as well as other parts of the endpoint process and the business logic. 

In scenarios where the concurrent message processing limit is changed, or the database connection is used for other purposes as mentioned above, it is advisable to change the connection pool size to ensure it will not be exhausted. See also [SQL Server Connection Pooling and Configuration](https://docs.microsoft.com/en-us/dotnet/framework/data/adonet/sql-server-connection-pooling).

partial: pool-size  


## Connection configuration

Connection string can be configured in several ways:

partial: connection-string


partial: multi-instance


## Custom database schemas

SQL Server transport uses `dbo` as a default schema. Default schema is used for every queue if no other schema is explicitly provided in transport address. That includes all local queues, error, audit and remote queues of other endpoints.

partial: custom-schema

## Custom SQL Server transport connection factory

In some environments it might be necessary to adapt to database server settings, or to perform additional operations. For example, if the `NOCOUNT` setting is enabled on the server, then it is necessary to send the `SET NOCOUNT OFF` command right after opening the connection.

That can be done by passing the transport a custom factory method which will provide connection strings at runtime, and which can perform custom actions:

snippet: sqlserver-custom-connection-factory

NOTE: If opening the connection fails, the custom connection factory must dispose the connection object and rethrow the exception.


## Circuit Breaker

A built in circuit breaker is used to handle intermittent SQL Server connectivity problems. When a failure occurs when trying to connect, a circuit breaker enters an *armed* state. If the failure is not resolved before the configured *wait time* elapses, the circuit breaker triggers the [critical errors](/nservicebus/hosting/critical-errors.md) handling procedure.

partial: circuit-breaker
