---
title: Transactional Session with SQL Persistence
summary: How to configure the transactional session with SQL Persistence
component: TransactionalSession
reviewed: 2022-09-12
redirects:
related:
- persistence/sql
---

In order to use the TransactionalSession feature with SQL Persistence, add a reference to the `NServiceBus.Persistence.Sql.TransactionalSession` NuGet package.

## Configuration

To enable the TransactionalSession feature:

snippet: enabling-transactional-session-sqlp

## Opening a session

To open a SQL Persistence transactional session:

snippet: open-transactional-session-sqlp

### Multi-tenancy support

The specific tenant id that is used to construct the connection string is retrieved from message headers as configured in the [`MultiTenantConnectionBuilder`-method](/persistence/sql/multi-tenant.md).
This header needs to be set in the options so that the necessary information is available when storing operations and interacting with the outbox.

snippet: open-transactional-session-sqlp-multitenant

## Transaction usage

Message and database operations made via the the transactional session are committed together once the session is committed:

snippet: use-transactional-session-sqlp

See the [SQL shared session documentation](/persistence/sql/accessing-data.md) for further details about using the transaction.

include: ts-outbox-warning