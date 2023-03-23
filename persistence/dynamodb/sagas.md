---
title: Sagas (DynamoDB)
summary: How to configure Saga persistence in DynamoDB
component: DynamoDB
reviewed: 2023-03-16
related:
- persistence/dynamodb
- nservicebus/sagas
---

## Configure Saga table

The Saga data table can be configured via:

snippet: DynamoSagaTableConfiguration

NOTE: When using the same table for saga and outbox data, use the [shared table configuration API](/persistence/dynamodb/#usage-customizing-the-table-used) instead.

## Saga concurrency

The DynamoDB saga persister uses optimistic concurrency control by default. Concurrently processed messages modifying the same saga will fail when the saga transaction completes after executing the message handler. In high-contention scenarios, pessimistic locking can enforce sequential access to the same saga to avoid concurrency related retries. To enable pessimistic locking, use:

snippet: DynamoDBSagaPessimisticLocking

For more information, refer to the [Saga concurrency documentation](/nservicebus/sagas/concurrency.md).