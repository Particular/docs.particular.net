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

## Transaction usage

Message and database operations made via the the transactional session are committed together once the session is committed:

snippet: use-transactional-session-mongo

See the [MongoDB persistence transactions documentation](/persistence/mongodb/#transactions) for further details about using the MongoDB transaction.

WARN: In order to guarantee atomic consistency across message and database operations, the [Outbox](/nservicebus/outbox) needs to be enabled. Otherwise `Commit` will execute all operations in a best-effort fashion.