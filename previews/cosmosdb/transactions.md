---
title: Transactions in Azure Cosmos DB persistence
component: CosmosDB
related:
- nservicebus/outbox
reviewed: 2020-09-11
---

By default, the persister does not attempt to atomically commit saga data and/or business data and uses the saga id as partition key to store sagas. Through the use of the [Cosmos DB transactional batch API](https://devblogs.microsoft.com/cosmosdb/introducing-transactionalbatch-in-the-net-sdk/), saga data and/or business data can be atomically committed if everything is stored in the same partition within a container.

A custom [behavior](/nservicebus/pipeline/manipulate-with-behaviors.md) must be introduced to identify and insert the [`PartitionKey`](https://docs.microsoft.com/en-us/dotnet/api/microsoft.azure.documents.partitionkey?view=azure-dotnet) value into the pipeline context for use by storage operations that occur during the processing of a given message.

INFO: Do not use a [message mutator](/nservicebus/pipeline/message-mutators.md) to identify the partition key. Message mutators do not offer the necessary control or timing to reliably interact with this persistence.

The custom behavior can be introduced in one of the two stages:

## `ITransportReceiveContext` stage

This is the earliest stage in the message processing pipeline. At this stage only the message ID, the headers and a byte array representation of the message body are available. 

snippet: ITransportReceiveContextBehavior

### Interaction with outbox

This stage occurs before the [outbox](/nservicebus/outbox) logic is executed. Identifying the [`PartitionKey`](https://docs.microsoft.com/en-us/dotnet/api/microsoft.azure.documents.partitionkey?view=azure-dotnet) during this stage allows the outbox feature to work entirely as intended. In the case where the `PartitionKey` cannot be identified using only the message headers, a behavior in the `IIncomingLogicalMessageContext` stage can be introduced instead.

## `IIncomingLogicalMessageContext` stage

This is the first stage in the pipeline that allows access to the deserialized message body. At this stage both the message headers and deserialized message object are available.

snippet: IIncomingLogicalMessageContextBehavior

### Interaction with outbox

[Outbox](/nservicebus/outbox) message guarantees work by bypassing the remaining message processing pipeline when an outbox record is located. Since this stage occurs after the normal bypass logic is executed, no [`PartitionKey`](https://docs.microsoft.com/en-us/dotnet/api/microsoft.azure.documents.partitionkey?view=azure-dotnet) is available to locate an existing outbox record. 

Cosmos DB persistence introduces a new `LogicalOutboxBehavior` to locate the outbox record and bypass the remaining message processing pipeline in the same `IIncomingLogicalMessageContext` stage as the custom `PartitionKey` behavior. As a result, the custom behavior must be inserted into the pipeline _before_ the `LogicalOutboxBehavior`.

To specify the ordering for the custom `PartitionKey` behavior:

snippet: InsertBeforeLogicalOutbox

To register the custom `PartitionKey` behavior:

snippet: CosmosDBRegisterLogicalBehavior

WARN: Caution must be used when custom behaviors have been introduced in the pipeline that [dispatch messages immediately](/nservicebus/messaging/send-a-message.md#dispatching-a-message-immediately). If these behaviors execute before the `LogicalOutboxBehavior` the [outbox message guarantees](/nservicebus/outbox/#how-it-works) may be broken.

## Sharing the transaction

Once a behavior is introduced to identify the partition key for a given message, it is possible to share a Cosmos DB [TransactionalBatch](https://docs.microsoft.com/en-us/dotnet/api/microsoft.azure.cosmos.transactionalbatch?view=azure-dotnet) between both the Saga persistence and business data. The shared `TransactionalBatch` can then be used to persist document updates for both concerns atomically.

NOTE: The shared [`TransactionalBatch`](https://docs.microsoft.com/en-us/dotnet/api/microsoft.azure.cosmos.transactionalbatch?view=azure-dotnet) will not perform any actions when [`ExecuteAsync()`](https://docs.microsoft.com/en-us/dotnet/api/microsoft.azure.cosmos.transactionalbatch.executeasync?view=azure-dotnet) is called. This allows NServiceBus to safely manage the unit of work. `ExecuteAsync` does not need to be called within the handler. All stream resources passed to the shared transaction will be properly disposed when NServiceBus executes the batch.

### Within a handler method using `IMessageHandlerContext`

To use the shared `TransactionalBatch` in a message handler:

snippet: CosmosDBHandlerSharedTransaction

#### Testing

The `TestableCosmosSynchronizedStorageSession` class in the `NServiceBus.Testing` namespace has been provided to facilitate [testing a handler](/nservicebus/testing/) that utilizes the shared transaction feature.

### With dependency injection

For custom types that require access to the shared `TransactionalBatch`:

snippet: TransactionalBatchRegisteredWithDependencyInjectionResolvedInCustomType

And alternatively to using the the extension method `IMessageHandlerContext.SynchronizedStorageSession.GetSharedTransactionalBatch()`:

snippet: TransactionalBatchRegisteredWithDependencyInjectionResolvedInHandler