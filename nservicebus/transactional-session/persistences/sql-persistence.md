---
title: Transactional Session with SQL Persistence
summary: How to configure the transactional session with SQL Persistence
component: TransactionalSession.SqlPersistence
reviewed: 2026-08-20
redirects:
related:
- persistence/sql
- nservicebus/transactional-session
- samples/transactional-session/aspnetcore-webapi
---

In order to use the [transactional session feature](/nservicebus/transactional-session/) with SQL Persistence, add a reference to the `NServiceBus.Persistence.Sql.TransactionalSession` NuGet package.

## Configuration

To enable the transactional session feature:

snippet: enabling-transactional-session-sqlp

## Opening a session

To open a SQL Persistence transactional session:

snippet: open-transactional-session-sqlp

### Multi-tenancy support

The specific tenant ID that is used to construct the connection string is retrieved from message headers as configured in the [`MultiTenantConnectionBuilder`-method](/persistence/sql/multi-tenant.md).
This header needs to be set in the options so that the necessary information is available when storing operations and interacting with the outbox.

snippet: open-transactional-session-sqlp-multitenant

## Transaction usage

Message and database operations made via the transactional session are committed together once the session is committed:

snippet: use-transactional-session-sqlp

See the [SQL shared session documentation](/persistence/sql/accessing-data.md) for further details about using the transaction.

include: ts-outbox-warning

#if-version [8.2,)
## Remote processor

When the transactional session is configured with a [remote processor](/nservicebus/transactional-session/#remote-processor), the send-only endpoint writes outbox records to the processor endpoint's outbox table, `[schema].[{tablePrefix}OutboxData]`. The send-only endpoint doesn't create or clean up an outbox table of its own.

The send-only endpoint works out the table name from its own configuration. The [table prefix](/persistence/sql/install.md#table-prefix) defaults to the processor endpoint name, with periods replaced by underscores, rather than the send-only endpoint name. To make sure the send-only endpoint writes to the table that the processor reads:

- Connect both endpoints to the same database.
- Use the same schema on both endpoints.
- If the processor endpoint has a custom table prefix, configure the same prefix on the send-only endpoint. Otherwise, don't configure a table prefix on the send-only endpoint.

> [!WARNING]
> If the send-only endpoint writes to a different table and that table exists, `Commit` succeeds, but the processor never finds the outbox record. After the [maximum commit duration](/nservicebus/transactional-session/#advanced-configuration-maximum-commit-duration) elapses, the processor consumes the dispatch message and logs a warning, and the outgoing messages are never dispatched. If the table doesn't exist, `Commit` throws an exception.
#end-if
