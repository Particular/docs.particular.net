---
title: Transactional Session with CosmosDB Persistence
summary: How to configure the transactional session with CosmosDB Persistence
component: TransactionalSession
reviewed: 2022-09-12
redirects:
---

In order to use the TransactionalSession feature with CosmosDB Persistence, add a reference to the `NServiceBus.Persistence.CosmosDB.TransactionalSession` NuGet package.

## Configuration

To enable the TransactionalSession feature:

snippet: enabling-transactional-session-cosmos

## Opening a session

To open a CosmosDB transactional session, a partition key must be provided:

snippet: open-transactional-session-cosmos

## Custom container

By default, the transactional session uses the [configured default container](/persistence/cosmosdb/#usage-customizing-the-container-used). The `CosmosOpenSessionOptions` can be configured with container information to be used for this transaction:

snippet: open-transactional-session-cosmos-container