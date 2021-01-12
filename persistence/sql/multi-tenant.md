---
title: Multi-tenant support
summary: SQL Persistence support for multi-tenant systems using database per customer
reviewed: 2021-01-11
component: SqlPersistence
versions: '[4.5,)'
related:
 - nservicebus/pipeline/manipulate-with-behaviors
 - samples/multi-tenant/sqlp
 - samples/multi-tenant/propagation
---

When working in a multi-tenant system, the data for each customer/client (tenant) is stored in an independent database identified by one or more message headers passed along with each message flowing through the system. With SQL Persistence running in multi-tenant mode, [saga data](saga.md) and [outbox data](outbox.md) are both stored in the same database as the tenant data, requiring only one database connection and transaction for the duration of the message handler.

[Timeout data](timeouts.md) and [subscription data](subscriptions.md) do not belong to any specific tenant and are stored in a shared database in cases where the message transport does not provide native timeouts or native publish/subscribe capabilities.

## Specifying connections per tenant

If the tenant information is propagated in a single header that does not change, multi-tenancy can be enabled by specifying the header name and a callback to create a connection given a tenant id:

snippet: MultiTenantWithHeaderName

In more complex situations, where the tenant id must be calculated by consulting multiple headers, or where a transition from an old header name to a new header name is occurring, a callback can be provided that captures the tenant id from the incoming message.

In this example, the header `NewTenantHeaderName` is consulted first, with `OldTenantHeaderName` as a backup.

snippet: MultiTenantWithFunc

NOTE: A null tenant id indicates a failure to propagate the tenant id from a previous message, rendering the message invalid. The recommended practice is to return null from the callback, rather than processing a message without proper tenant id. In such a case, SQL Persistence will throw an exception, and the message will be moved to the error queue.

## Disabling Outbox cleanup

When using the [Outbox feature](/nservicebus/outbox/) with a single database, the endpoint will [clean up its own deduplication data](outbox.md#deduplication-record-lifespan). When using multi-tenant mode, it's impossible for the endpoint to know all the possible tenant databases it must clean up. If using the Outbox with multi-tenant mode, the cleanup process must be disabled and implemented as a SQL Agent (or similar) task, otherwise the following exception will be thrown at runtime:

> MultiTenantConnectionBuilder can only be used with the Outbox feature if Outbox cleanup is handled by an external process (i.e. SQL Agent) and the endpoint is configured to disable Outbox cleanup using endpointConfiguration.EnableOutbox().DisableCleanup(). See the SQL Persistence documentation for more information on how to clean up Outbox tables from a scheduled task.

This opt-in approach ensures the user is not taken by surprise by the need to self-clean the Outbox tables. This approach also gives the advantage of being able to schedule the cleanup process to a slow time of day for that customer, or to optimize performance by running Outbox cleanup right before rebuilding database indexes.

To disable the Outbox cleanup so that multi-tenant mode can be used with the Outbox enabled, add this configuration:

snippet: DisableOutboxForMultiTenant

The following snippet shows a script written in T-SQL (Microsoft SQL Server) that cleans the Outbox table for a single endpoint:

snippet: MultiTenantOutboxCleanup

Since each endpoint uses a separate Outbox table, a database cursor over Outbox table names can be used to clean all endpoints' outbox tables at once:

snippet: MultiTenantMultiEndpointOutboxCleanup

The cleanup script would be similar for other database engines. Refer to the default Outbox cleanup scripts for [MySQL](mysql-scripts.md#run-time-outbox), [PostgreSQL](postgresql-scripts.md#run-time-outbox), and [Oracle](oracle-scripts.md#run-time-outbox) to get an idea of the operation that needs to be scripted.

## Propagating tenant id headers

For a system to be multi-tenant, every endpoint must use an [NServiceBus pipeline behavior](/nservicebus/pipeline/manipulate-with-behaviors.md) so that the tenant id header(s) is copied from the incoming message to every outgoing message.

If such a behavior does not exist, it will result in the endpoint being unable to determine the tenant id from an incoming message, causing following exceptionto be thrown:

> This endpoint attempted to process a message in multi-tenant mode and was unable to determine the tenant id from the incoming message. As a result SQL Persistence cannot determine which tenant database to use. Either: 1) The message lacks a tenant id and is invalid. 2) The lambda provided to determine the tenant id from an incoming message contains a bug. 3) Either this endpoint or another upstream endpoint is not configured to use a custom behavior for relaying tenant information from incoming to outgoing messages, or that behavior contains a bug.

Refer to the [Propagating Tenant Information to Downstream Endpoints](/samples/multi-tenant/propagation/) sample to see how to create and register pipeline behaviors to propagate the tenant id to downstream endpoints.

## Connections for timeouts and subscriptions

When using multi-tenant mode, [timeouts](timeouts.md) and [subscriptions](subscriptions.md) are stored in a single database if the message transport does not provide those features (delayed delivery and publish/subscribe) natively.

If these persistence features are used, but a connection builder is not specified, the following exception will be thrown:

> Couldn't find connection string for {storageType.Name}. The connection to the database must be specified using the `ConnectionBuilder` method. When in multi-tenant mode with `MultiTenantConnectionBuilder`, you must still use `ConnectionBuilder` to provide a database connection for subscriptions/timeouts on message transports that don't support those features natively.

To specify the connection builder for timeouts or subscriptions, refer to the usage documentation for [Microsoft SQL](dialect-mssql.md#usage), [MySQL](dialect-mysql.md#usage), [PostgreSQL](dialect-postgresql.md#usage), or [Oracle](dialect-oracle.md#usage).

When using a transport with both native delayed delivery and native timeouts, this is not required and no exception will be thrown.

## Cannot use installers

Because tenant databases are not known at endpoint startup, it's impossible for NServiceBus to run installers to create table structures in each tenant database.

When using multi-tenant endpoints it's advisable to:

1. Use [script promotion](controlling-script-generation.md#promotion) to copy DDL scripts outside of the runtime directory and commit them to source control.
2. Create a [process to execute the DDL scripts](installer-workflow.md#contrasting-workflows-higher-environment-workflow) against all required databases as part of each deployment.
