---
title: Transactional Session with MongoDB Persistence
summary: How to configure the transactional session with MongoDB Persistence
component: TransactionalSession
reviewed: 2022-09-12
redirects:
related:
- persistence/mongodb
---

In order to use the TransactionalSession feature with MongoDB persistence, add a reference to the `NServiceBus.Storage.MongoDB.TransactionalSession` NuGet package.

## Configuration

To enable the TransactionalSession feature:

snippet: enabling-transactional-session-mongo

## Opening a session

To open a MongoDB transactional session:

snippet: open-transactional-session-mongo