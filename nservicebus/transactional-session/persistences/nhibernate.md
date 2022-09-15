---
title: Transactional Session with NHibernate Persistence
summary: How to configure the transactional session with NHibernate Persistence
component: TransactionalSession
reviewed: 2022-09-12
redirects:
related:
- persistence/nhibernate
---

In order to use the TransactionalSession feature with NHibernate Persistence, add a reference to the `NServiceBus.NHibernate.TransactionalSession` NuGet package.

## Configuration

To enable the TransactionalSession feature:

snippet: enabling-transactional-session-nhibernate

## Opening a session

To open a NHibernate transactional session:

snippet: open-transactional-session-nhibernate

## Transaction usage

Message and database operations made via the the transactional session are committed together once the session is committed:

snippet: use-transactional-session-nhibernate

See the [NHibernate shared session documentation](/persistence/nhibernate/accessing-data.md) for further details about using the transaction.

WARN: In order to guarantee atomic consistency across message and database operations, the [Outbox](/nservicebus/outbox) needs to be enabled. Otherwise `Commit` will execute all operations in a best-effort fashion.