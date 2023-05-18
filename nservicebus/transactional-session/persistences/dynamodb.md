---
title: Transactional Session with DynamoDB Persistence
summary: How to configure a transactional session with DynamoDB persistence
component: TransactionalSession.DynamoDB
reviewed: 2022-09-12
redirects:
related:
- persistence/dynamodb
- nservicebus/transactional-session
---

In order to use the [transactional session feature](/nservicebus/transactional-session/) with DynamoDB persistence, add a reference to the `NServiceBus.Persistence.DynamoDB.TransactionalSession` NuGet package.

## Configuration

To enable the transactional session feature:

snippet: enabling-transactional-session-dynamo

## Opening a session

To open a DynamoDB transactional session:

snippet: open-transactional-session-dynamo

## Transaction usage

Message and database operations made with the transactional session are committed together once the session is committed:

snippet: use-transactional-session-dynamo

See the [Cosmos DB persistence transactions documentation](/persistence/dynamodb/transactions.md) for further details about using the transaction.

include: ts-outbox-warning
