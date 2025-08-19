---
title: Transactional Session with CosmosDB Persistence
summary: How to configure a transactional session with CosmosDB Persistence
component: TransactionalSession.CosmosDB
reviewed: 2025-01-24
redirects:
related:
- persistence/cosmosdb
- nservicebus/transactional-session
- samples/transactional-session/cosmosdb
---

In order to use the [transactional session feature](/nservicebus/transactional-session/) with CosmosDB Persistence, add a reference to the `NServiceBus.Persistence.CosmosDB.TransactionalSession` NuGet package.

## Configuration

To enable the transactional session feature:

snippet: enabling-transactional-session-cosmos

## Opening a session

To open a CosmosDB transactional session, a partition key must be provided:

snippet: open-transactional-session-cosmos

### Custom container

By default, the transactional session uses the [configured default container](/persistence/cosmosdb/#usage-customizing-the-container-used). The `CosmosOpenSessionOptions` instance can be configured with container information to be used for this transaction:

snippet: open-transactional-session-cosmos-container

## Transaction usage

Message and database operations made via the the transactional session are committed together once the session is committed:

snippet: use-transactional-session-cosmos

See the [Cosmos DB persistence transactions documentation](/persistence/cosmosdb/transactions.md#sharing-the-transaction) for further details about using the transaction.

include: ts-outbox-warning
