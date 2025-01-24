---
title: Transactional Session with SQL Persistence
summary: How to configure the transactional session with SQL Persistence
component: TransactionalSession.SqlPersistence
reviewed: 2025-01-24
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
