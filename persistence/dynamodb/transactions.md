---
title: DynamoDB persistence transactions
summary: How to use transactions with DynamoDB persistence
component: DynamoDB
reviewed: 2023-03-16
related:
- persistence/dynamodb
---

Outbox and Saga persistence commit their changes transactionally, using the [DynamoDB TransactWriteItems](https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/transactions.html) API. Message handlers can add further operations to this transaction via the synchronized session:

snippet: DynamoDBSynchronizedSession

Transactions can contain a maximum of 100 operations. This limit is shared with operations enlisted by NServiceBus. Each saga will use one operation. Outbox will use `1 + <amount of outgoing messages>` operations.

## Testing

When [unit testing](/samples/unit-testing/) a message handler, the `TestableDynamoDBSynchronizedStorageSession` class can be used:

snippet: DynamoDBTestingSessionUsage