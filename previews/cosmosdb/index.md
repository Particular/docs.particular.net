---
title: Azure Cosmos DB Persistence
summary: How to use NServiceBus with Azure Cosmos DB
component: CosmosDB
reviewed: 2020-09-11
related:
- samples/previews/cosmosdb/transactions
- samples/previews/cosmosdb/container
- samples/previews/cosmosdb/simple
---

Uses the [Azure Cosmos DB](https://azure.microsoft.com/en-us/services/cosmos-db/) NoSQL database service for storage.

WARN: It is important to [read and understand](https://docs.microsoft.com/en-us/azure/cosmos-db/partitioning-overview) partitioning in Azure Cosmos DB before using `NServiceBus.Persistence.CosmosDB`.

## Persistence at a glance

For a description of each feature, see the [persistence at a glance legend](/persistence/#persistence-at-a-glance).

|Feature                    |   |
|:---                       |---
|Supported storage types    |Sagas, Outbox
|Transactions               |Using TransactionalBatch, [with caveats](transactions.md)
|Concurrency control        |Optimistic concurrency
|Scripted deployment        |Not supported
|Installers                 |Container is created by installers.

NOTE: The Outbox feature requires partition planning.

## Usage

Add a NuGet package reference to `NServiceBus.Persistence.CosmosDB`. Configure the endpoint to use the persistence through the following configuration API:

snippet: CosmosDBUsage

### Customizing the database used

By default, the persister will store records in a database named `NServiceBus` and use a container per endpoint using the endpoint name as to name the container.

Customize the database name using the following configuration API:

snippet: CosmosDBDatabaseName

### Customizing the container used

Setting the default container used using the following configuration API

snippet: CosmosDBDefaultContainer

When installers are enabled the default container will be created if it doesn't exist. To opt-out from creating the default container either disable the installers or use

snippet: CosmosDBDisableContainerCreation

#### Advanced container customization

WARN: When the container name and partition key path are provided at runtime it takes precedence over any default container configured using [configuration API](#usage-customizing-the-container-used). The container specified at runtime has to exist and be configured properly to order to work.

The container name can be provided using a custom behavior at the physical stage

snippet: CustomContainerNameUsingITransportReceiveContextBehavior

or at the logical stage

snippet: CustomContainerNameUsingIIncomingLogicalMessageContextBehavior

### Customizing the CosmosClient provider

In cases when the CosmosClient is configured and used via dependency injection a custom provider can be implemented

snippet: CosmosDBCustomClientProvider

and registered on the container

snippet: CosmosDBCustomClientProviderRegistration

## Transactions

The Cosmos DB persister supports using the [Cosmos DB transactional batch API](https://devblogs.microsoft.com/cosmosdb/introducing-transactionalbatch-in-the-net-sdk/). However, Cosmos DB only allows operations to be batched if all operations are performed within the same logical partition key. This is due to the distributed nature of the Cosmos DB service, which [does not support distributed transactions](/nservicebus/azure/understanding-transactionality-in-azure.md).

The [transactions](transactions.md) documentation provides additional details on how to configure NServiceBus to resolve the incoming message to a specific partition key to take advantage of this Cosmos DB feature.

## Outbox cleanup 

When the outbox is enabled, the deduplication data is kept for seven days by default. To customize this time frame, use the following API:

snippet: CosmosDBOutboxCleanup

Outbox cleanup depends on the Cosmos DB time-to-live feature. Failure to remove the expired outbox records is caused by a misconfigured collection that has time-to-live disabled. Refer to the [Cosmos DB documentation](https://docs.microsoft.com/en-us/azure/cosmos-db/time-to-live) to configure the collection correctly.

## Saga concurrency

Due to the distributed nature of Cosmos DB [optimistic concurrency control](https://en.wikipedia.org/wiki/Optimistic_concurrency_control) is always used when updating or deleting saga data. When simultaneously handling messages, conflicts may occur. See below for examples of the exceptions which are thrown. _[Saga concurrency](/nservicebus/sagas/concurrency.md)_ explains how these conflicts are handled, and contains guidance for high-load scenarios.

include: saga-concurrency

### Starting a saga

Example exception:

```
The 'OrderSagaData' saga with id '7ac4d199-6560-4d1a-b83a-b3dad94b0802' could not be created possibly due to a concurrency conflict.
```

### Updating or deleting saga data

Example exception:

```
The 'OrderSagaData' saga with id '7ac4d199-6560-4d1a-b83a-b3dad94b0802' was updated by another process or no longer exists.
```
