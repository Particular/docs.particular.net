---
title: Transactions in Azure Cosmos DB persistence
component: CosmosDB
related:
- nservicebus/outbox
reviewed: 2020-09-11
redirects:
- previews/cosmosdb/transactions
---

By default, the persister does not attempt to atomically commit saga data and/or business data and uses the saga id as partition key to store sagas. Through the use of the [Cosmos DB transactional batch API](https://devblogs.microsoft.com/cosmosdb/introducing-transactionalbatch-in-the-net-sdk/), saga data and/or business data can be atomically committed if everything is stored in the same partition within a container.

partial: information

## Sharing the transaction

It is possible to share a Cosmos DB [TransactionalBatch](https://docs.microsoft.com/en-us/dotnet/api/microsoft.azure.cosmos.transactionalbatch?view=azure-dotnet) between both the Saga persistence and business data. The shared `TransactionalBatch` can then be used to persist document updates for both concerns atomically.

NOTE: The shared [`TransactionalBatch`](https://docs.microsoft.com/en-us/dotnet/api/microsoft.azure.cosmos.transactionalbatch?view=azure-dotnet) will not perform any actions when [`ExecuteAsync()`](https://docs.microsoft.com/en-us/dotnet/api/microsoft.azure.cosmos.transactionalbatch.executeasync?view=azure-dotnet) is called. This allows NServiceBus to safely manage the unit of work. `ExecuteAsync` does not need to be called within the handler. All stream resources passed to the shared transaction will be properly disposed when NServiceBus executes the batch.

### Within a handler method using `IMessageHandlerContext`

To use the shared `TransactionalBatch` in a message handler:

snippet: CosmosDBHandlerSharedTransaction

#### Testing

The `TestableCosmosSynchronizedStorageSession` class in the `NServiceBus.Testing` namespace has been provided to facilitate [testing a handler](/nservicebus/testing/) that utilizes the shared transaction feature.

### With dependency injection

For custom types that require access to the shared `TransactionalBatch`:

snippet: CosmosDB-TransactionalBatchRegisteredWithDependencyInjectionResolvedInCustomType

And alternatively to using the the extension method `IMessageHandlerContext.SynchronizedStorageSession.GetSharedTransactionalBatch()`:

snippet: CosmosDB-TransactionalBatchRegisteredWithDependencyInjectionResolvedInHandler