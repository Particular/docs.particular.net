---
title: Transactions in Azure Cosmos DB persistence
component: CosmosDB
related:
- nservicebus/outbox
reviewed: 2024-10-15
redirects:
- previews/cosmosdb/transactions
---

By default, the persister does not attempt to atomically commit saga data and/or business data and uses the saga id as partition key to store sagas. Through the use of the [Cosmos DB transactional batch API](https://devblogs.microsoft.com/cosmosdb/introducing-transactionalbatch-in-the-net-sdk/), saga data and/or business data can be atomically committed if everything is stored in the same partition within a container.

The Cosmos DB persistence provides the several ways to specify the partition and Container used per message using message headers or the message contents to allow the use of transactions.

Using message headers only has the advantage being able to identify the partition or `Container` before the [outbox](/nservicebus/outbox) logic is executed, which allows the outbox feature to work entirely as intended.

In the case where the partition or Container cannot be identified using only the message headers, the message contents can be used. This works because the Cosmos DB persistence introduces additional outbox logic to locate the outbox record and bypass the remaining message processing pipeline at a later stage of processing.

## Specifying the `PartitionKey` to use for the transaction

All operations in a Azure Cosmos DB transaction must be executed with documents contained in the same `Container` partition, identified by the `PartitionKey`.

### Using message header values

A single message header value can be used to specify the `PartitionKey` for the partition:

snippet: ExtractPartitionKeyFromHeaderSimple

Multiple message header values can also be used. Additionally overloads exist that allow a state object to be provided and passed when the extractor is called to avoid unnecessary allocations:

snippet: ExtractPartitionKeyFromHeadersExtractor

A custom class that implements `IPartitionKeyFromHeadersExtractor` can be implemented to specify the `PartitionKey` using message headers:

snippet: CustomPartitionKeyFromHeadersExtractor

The `IPartitionKeyFromHeadersExtractor` implementation can be configured via the API:

snippet: ExtractPartitionKeyFromHeadersCustom

or registered via dependency injection:

snippet: ExtractPartitionKeyFromHeadersRegistration

include: explicit_before_di_note

Besides those API methods shown here, additional overloads are available for extracting `PartitionKey`.

### Using the message contents

The message contents can be accessed to specify the `PartitionKey` of the partition for the transaction:

snippet: ExtractPartitionKeyFromMessageExtractor

A custom class that implements `IPartitionKeyFromMessageExtractor` can be implemented that can access the message contents and headers to specify the partition to use for the transaction:

snippet: CustomPartitionKeyFromMessageExtractor

The `IPartitionKeyFromMessageExtractor` implementation can be configured using the API:

snippet: ExtractPartitionKeyFromMessageCustom

or registered via dependency injection:

snippet: ExtractPartitionKeyFromMessageRegistration

include: explicit_before_di_note

Additional overloads are available for extracting `PartitionKey`.

## Specifying the `Container` to use for the transaction

The Container to use can be specified by defining a default container:

include: defaultcontainer

Optionally, the `Container` to use can specified during message processing by providing the `Container` name and partition key path using the `ContainerInformation` object.

include: containeroverride

### Using message header values

The presence of a header value can be used to specify the container:

snippet: ExtractContainerInfoFromHeaderInstance

A single message header value can be used to specify the container:

snippet: ExtractContainerInfoFromHeader

Multiple message header values can also be used. Additionally overloads exist that allow a state object to be passed when the extractor is called to avoid unnecessary allocations:

snippet: ExtractContainerInfoFromHeaders

A custom class that implements `IContainerInformationFromHeadersExtractor` can be implemented to specify the `Container` using message headers:

snippet: CustomContainerInformationFromHeadersExtractor

The `IContainerInformationFromHeadersExtractor` implementation can be configured using the API:

snippet: ExtractContainerInfoFromHeadersCustom

or registered via dependency injection:

snippet: ExtractContainerInfoFromHeadersRegistration

include: explicit_before_di_note

Besides those API methods shown here, additional overloads are available for extracting `ContainerInformation` from headers.

### Using the message contents

A container can be specified on a per-message type basis:

snippet: ExtractContainerInfoFromMessageInstance

The message contents can be accessed to specify the container to use for the transaction:

snippet: ExtractContainerInfoFromMessageExtractor

A custom class that implements `IContainerInformationFromMessagesExtractor` can be implemented that makes use of the messages and headers to specify the container to use for the transaction:

snippet: CustomContainerInformationFromMessageExtractor

The `IContainerInformationFromMessagesExtractor` implementation can be configured using the API:

snippet: ExtractContainerInfoFromMessageCustom

or registered via dependency injection:

snippet: ExtractContainerInfoFromMessageRegistration

include: explicit_before_di_note

Additional overloads are available for extracting `ContainerInformation` from the message.

## Sharing the transaction

It is possible to share a Cosmos DB [TransactionalBatch](https://docs.microsoft.com/en-us/dotnet/api/microsoft.azure.cosmos.transactionalbatch?view=azure-dotnet) between both the Saga persistence and business data. The shared `TransactionalBatch` can then be used to persist document updates for both concerns atomically.

> [!NOTE]
> The shared [`TransactionalBatch`](https://docs.microsoft.com/en-us/dotnet/api/microsoft.azure.cosmos.transactionalbatch?view=azure-dotnet) will not perform any actions when [`ExecuteAsync()`](https://docs.microsoft.com/en-us/dotnet/api/microsoft.azure.cosmos.transactionalbatch.executeasync?view=azure-dotnet) is called. This allows NServiceBus to safely manage the unit of work. `ExecuteAsync` does not need to be called within the handler. All stream resources passed to the shared transaction will be properly disposed when NServiceBus executes the batch.

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