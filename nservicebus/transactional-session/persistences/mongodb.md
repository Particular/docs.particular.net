---
title: Transactional Session with MongoDB Persistence
summary: How to configure the transactional session with MongoDB Persistence
component: TransactionalSession.MongoDB
reviewed: 2025-01-24
redirects:
related:
- persistence/mongodb
- nservicebus/transactional-session
---

In order to use the [transactional session feature](/nservicebus/transactional-session/) with MongoDB persistence, add a reference to the `NServiceBus.Storage.MongoDB.TransactionalSession` NuGet package.

## Configuration

To enable the transactional session feature:

snippet: enabling-transactional-session-mongo

## Opening a session

To open a MongoDB transactional session:

snippet: open-transactional-session-mongo

## Transaction usage

Message and database operations made via the transactional session are committed together once the session is committed:

snippet: use-transactional-session-mongo

See the [MongoDB persistence transactions documentation](/persistence/mongodb/#transactions) for further details about using the MongoDB transaction.

include: ts-outbox-warning
