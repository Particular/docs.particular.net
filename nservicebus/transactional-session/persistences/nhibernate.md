---
title: Transactional Session with NHibernate Persistence
summary: How to configure the transactional session with NHibernate Persistence
component: TransactionalSession
reviewed: 2022-09-12
versions: "[7,)"
redirects:
---

In order to use the TransactionalSession feature with NHibernate Persistence, add a reference to the `NServiceBus.NHibernate.TransactionalSession` NuGet package.

## Configuration

To enable the TransactionalSession feature:

snippet: enabling-transactional-session-nhibernate

## Opening a session

To open a NHibernate transactional session:

snippet: open-transactional-session-nhibernate
