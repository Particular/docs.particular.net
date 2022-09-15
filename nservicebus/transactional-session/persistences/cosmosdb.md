---
title: Transactional Session with CosmosDB Persistence
summary: How to configure the transactional session with CosmosDB Persistence
component: TransactionalSession
reviewed: 2022-09-12
redirects:
related:
- persistence/cosmosdb
---

In order to use the TransactionalSession feature with CosmosDB Persistence, add a reference to the `NServiceBus.Persistence.CosmosDB.TransactionalSession` NuGet package.

## Configuration

To enable the TransactionalSession feature:

snippet: enabling-transactional-session-cosmos

## Opening a session

To open a CosmosDB transactional session, a partition key must be provided:

snippet: open-transactional-session-cosmos

### Custom container

By default, the transactional session uses the [configured default container](/persistence/cosmosdb/#usage-customizing-the-container-used). The `CosmosOpenSessionOptions` can be configured with container information to be used for this transaction:

snippet: open-transactional-session-cosmos-container

## Transaction usage

Message and database operations made via the the transactional session are committed together once the session is committed:

snippet: use-transactional-session-cosmos

See the [Cosmos DB persistence transactions documentation](/persistence/cosmosdb/transactions.md#sharing-the-transaction) for further details about using the transaction.

WARN: In order to guarantee atomic consistency across message and database operations, the [Outbox](/nservicebus/outbox) needs to be enabled. Otherwise `Commit` will execute all operations in a best-effort fashion.