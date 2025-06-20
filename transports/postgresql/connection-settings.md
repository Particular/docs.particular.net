---
title: Connection Settings
summary: Information about the connection settings for the PostgreSQL transport, including custom database schemas and circuit breakers
reviewed: 2024-05-27
component: PostgreSqlTransport
---

## Connection pooling

The PostgreSQL transport is built on top of [ADO.NET](https://docs.microsoft.com/en-us/dotnet/framework/data/adonet/index) and will use connection pooling. This may result in sharing of the connection pool by the transport, as well as other parts of the endpoint process and the business logic.

If increasing the concurrent message processing limit, or if the database connection is used for other purposes mentioned above, increase the connection pool size to ensure it is not exhausted.

> [!NOTE]
> If the maximum pool size is not explicitly set on the connection string, a warning message will be logged. See also [Tuning endpoint message processing](/nservicebus/operations/tuning.md).

## Connection configuration

The connection string is configured in the constructor of the transport object:

snippet: postgresql-config-connectionstring

### Token Authentication

To connect using token credentials, the following must be provided in the connection string:

- A User ID.
- The password taken from the access token.

Since tokens are short-lived, a [data source builder must be utilized to handle password refreshes](https://devblogs.microsoft.com/dotnet/using-postgre-sql-with-dotnet-and-entra-id/). The following example uses Microsoft Entra ID.

snippet: postgresql-config-entra

## Custom database schemas

The transport uses `public` as a default schema. It is used for every queue if no other schema is explicitly provided in a transport address. This includes all local queues, error, audit, and remote queues of other endpoints.

The default schema can be overridden using the `DefaultSchema` method:

snippet: postgresql-non-standard-schema

> [!NOTE]
> Subscribing to events between endpoints in different database schemas or catalogs requires a [shared subscription table to be configured](/transports/postgresql/native-publish-subscribe.md#configure-subscription-table).

## Custom connection factory

Some environments need additional connection operations, such as adapting to the database server settings. To achieve this, pass a custom factory method to the transport that will provide connection strings at runtime and can perform custom actions:

snippet: postgresql-custom-connection-factory

> [!NOTE]
> If opening the connection fails, the custom connection factory must dispose of the connection object and rethrow the exception.

> [!WARNING]
> When using custom schemas, ensure the connection returned by the connection factory is granted sufficient permissions for the endpoint to perform its operations.

## Circuit breaker

A built-in circuit breaker is used to handle intermittent PostgreSQL connectivity problems. When a failure occurs while trying to connect, a circuit breaker enters an _armed_ state. If the failure is not resolved before the configured _wait time_ elapses, the circuit breaker triggers the [critical errors](/nservicebus/hosting/critical-errors.md) handling procedure.

### Wait time

The circuit breaker's default time to wait before triggering is 30 seconds. Use the `TimeToWaitBeforeTriggeringCircuitBreaker` method to change it.

snippet: postgresql-TimeToWaitBeforeTriggeringCircuitBreaker
