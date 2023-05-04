---
title: Sagas (DynamoDB)
summary: How to configure Saga persistence in DynamoDB
component: DynamoDB
reviewed: 2023-03-16
related:
- persistence/dynamodb
- nservicebus/sagas
---

## Configure the Saga table

The Saga data table can be configured via:

snippet: DynamoSagaTableConfiguration

NOTE: When using the same table for saga and outbox data, use the [shared table configuration API](/persistence/dynamodb/#usage-customizing-the-table-used) instead.

## Saga data mapping

Saga data is automatically mapped using the built-in mapper described in the [transaction documentation](/persistence/dynamodb/transactions.md#mapping).

## Saga concurrency

The DynamoDB saga persister uses optimistic concurrency control by default. Concurrently processed messages modifying the same saga will fail when the saga transaction completes after executing the message handler. In high-contention scenarios, pessimistic locking can enforce sequential access to the same saga to avoid concurrency related retries. To enable pessimistic locking, use:

snippet: DynamoDBSagaPessimisticLocking

For more information, refer to the [Saga concurrency documentation](/nservicebus/sagas/concurrency.md).

### Pessimistic locking configuration

NOTE: The lease configuration options are advanced configuration options. It is recommended to change the default settings only when special requirements need to be met.

Pessimistic locking is implemented using [leases](https://en.wikipedia.org/wiki/Lease_(computer_science)). The lease duration determines the amount of time exclusive access is guaranteed before other readers are able to acquire a lease again. The default duration is 30 seconds. To change the lease duration:

snippet: DynamoDBLeaseDuration

If a saga data is already beinig locked via an active lease, the client will continue to acquire the lease for some time in case the lease will be released shortly. The default retry duration is 10 seconds and can be changed via:

snippet: DynamoDBLeaseAcquisitionTimeout
