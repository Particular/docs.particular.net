---
title: Sagas (DynamoDB)
summary: How to configure saga persistence in DynamoDB
component: DynamoDB
reviewed: 2025-04-24
related:
- persistence/dynamodb
- nservicebus/sagas
---

This article describes how to configure [NServiceBus sagas](/nservicebus/sagas/) for use with DynamoDB

## Configure the saga table

The saga data table can be configured as follows:

snippet: DynamoSagaTableConfiguration

> [!NOTE]
> When using the same table for saga and outbox data, use the [shared table configuration API](/persistence/dynamodb/#usage-customizing-the-table-used) instead.

## Saga data mapping

Saga data is automatically mapped using the built-in mapper described in the [transaction documentation](/persistence/dynamodb/transactions.md#mapping).

partial: options

## Saga concurrency

The DynamoDB saga persister uses optimistic concurrency control by default. Concurrently processed messages modifying the same saga will fail when the saga transaction completes after executing the message handler. In high-contention scenarios, pessimistic locking can enforce sequential access to the same saga to avoid concurrency related retries. To enable pessimistic locking, use:

snippet: DynamoDBSagaPessimisticLocking

For more information, refer to the [saga concurrency documentation](/nservicebus/sagas/concurrency.md).

### Pessimistic locking configuration

> [!NOTE]
> The lease configuration options are advanced configuration options. It is recommended to change the default settings only when special requirements need to be met.

Pessimistic locking is implemented using [leases](https://en.wikipedia.org/wiki/Lease_(computer_science)). The lease duration determines the amount of time exclusive access is guaranteed before other readers are able to acquire a lease again. The default duration is 30 seconds. To change the lease duration:

snippet: DynamoDBLeaseDuration

When a client attempts to acquire a lease on a saga data record that is locked, it will retry acquiring a lease for a configurable amount of time before timing out. The default retry duration is 10 seconds and can be changed with the following code:

snippet: DynamoDBLeaseAcquisitionTimeout

partial: consistency
