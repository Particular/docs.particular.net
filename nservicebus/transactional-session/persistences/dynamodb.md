---
title: Transactional Session with DynamoDB Persistence
summary: How to configure a transactional session with DynamoDB persistence
component: TransactionalSession.DynamoDB
reviewed: 2025-01-20
redirects:
related:
- persistence/dynamodb
- nservicebus/transactional-session
---

To use the [transactional session feature](/nservicebus/transactional-session/) with DynamoDB persistence, add a reference to the `NServiceBus.Persistence.DynamoDB.TransactionalSession` NuGet package.

## Configuration

To enable the transactional session feature:

snippet: enabling-transactional-session-dynamo

## Opening a session

To open a DynamoDB transactional session:

snippet: open-transactional-session-dynamo

## Transactions usage

Message and database operations made with the transactional session are committed together once the session is committed:

snippet: use-transactional-session-dynamo

For further details about using transactions, see the [DynamoDB persistence transactions documentation](/persistence/dynamodb/transactions.md).

include: ts-outbox-warning
