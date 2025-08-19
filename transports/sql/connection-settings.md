---
title: Connection Settings
summary: Information about the connection settings for the SQL Server transport, including custom database schemas and circuit breakers
reviewed: 2024-08-02
component: SqlTransport
redirects:
- nservicebus/sqlserver/connection-settings
- transports/sqlserver/connection-settings
---

## Using connection pooling

The SQL Server transport is built on top of [ADO.NET](https://docs.microsoft.com/en-us/dotnet/framework/data/adonet/index) and will use connection pooling. This may result in the connection pool being shared by the transport, as well as other parts of the endpoint process and the business logic.

In scenarios where the concurrent message processing limit is changed, or the database connection is used for other purposes mentioned above, change the connection pool size to ensure it will not be exhausted. See [SQL Server Connection Pooling and Configuration](https://docs.microsoft.com/en-us/dotnet/framework/data/adonet/sql-server-connection-pooling) for more details.

partial: pool-size

## Connection configuration

The connection string can be configured in several ways:

partial: connection-string

## Token-credentials

Microsoft Entra ID authentication is supported via the [standard connection string options](https://learn.microsoft.com/en-us/sql/connect/ado-net/sql/azure-active-directory-authentication).

## Custom database schemas

The SQL Server transport uses `dbo` as a default schema. It is used for every queue if no other schema is explicitly provided in a transport address. This includes all local queues, error, audit and remote queues of other endpoints.

partial: custom-schema

partial: custom-catalog

> [!NOTE]
> When subscribing to events between endpoints in different database schemas or catalogs, a [shared subscription table must be configured](/transports/sql/native-publish-subscribe.md#configure-subscription-table).

## Custom SQL Server transport connection factory

In some environments it might be necessary to adapt to the database server settings, or to perform additional operations. For example, if the `NOCOUNT` setting is enabled on the server, then it is necessary to send the `SET NOCOUNT OFF` command immediately after opening the connection.

This can be done by passing a custom factory method to the transport which will provide connection strings at runtime, and which can perform custom actions:

snippet: sqlserver-custom-connection-factory

> [!NOTE]
> If opening the connection fails, the custom connection factory must dispose the connection object and rethrow the exception.

> [!WARNING]
> When using custom schemas or catalogs, ensure the connection returned by the connection factory is granted sufficient permissions for the endpoint to perform its operations.

## Circuit breaker

A built-in circuit breaker is used to handle intermittent SQL Server connectivity problems. When a failure occurs while trying to connect, a circuit breaker enters an *armed* state. If the failure is not resolved before the configured *wait time* elapses, the circuit breaker triggers the [critical errors](/nservicebus/hosting/critical-errors.md) handling procedure.

### Wait time

The circuit breaker's default time to wait before triggering is 30 seconds. Use the `TimeToWaitBeforeTriggeringCircuitBreaker` method to change it.

snippet: sqlserver-TimeToWaitBeforeTriggeringCircuitBreaker

