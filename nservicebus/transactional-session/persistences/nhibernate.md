---
title: Transactional Session with NHibernate Persistence
summary: How to configure the transactional session with NHibernate Persistence
component: TransactionalSession.NHibernate
reviewed: 2025-01-24
redirects:
related:
- persistence/nhibernate
- nservicebus/transactional-session
---

In order to use the [transactional session feature](/nservicebus/transactional-session/) with NHibernate Persistence, add a reference to the `NServiceBus.NHibernate.TransactionalSession` NuGet package.

## Configuration

To enable the transactional session feature:

snippet: enabling-transactional-session-nhibernate

## Opening a session

To open a NHibernate transactional session:

snippet: open-transactional-session-nhibernate

## Transaction usage

Message and database operations made via the the transactional session are committed together once the session is committed:

snippet: use-transactional-session-nhibernate

See the [NHibernate shared session documentation](/persistence/nhibernate/accessing-data.md) for further details about using the transaction.

include: ts-outbox-warning
