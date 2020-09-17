---
title: Azure Cosmos DB Persistence
component: cosmosdb
reviewed: 2020-09-11
---

Uses the [Azure Cosmos DB](https://azure.microsoft.com/en-us/services/cosmos-db/) NoSQL database service for storage.

## Supported persistence types

* [Sagas](/nservicebus/sagas/)
* [Outbox](/nservicebus/outbox/)

NOTE: [Transactions](#transactions) are required to utilize the Outbox feature.

## Usage

Add a NuGet package reference to `NServiceBus.Persistence.CosmosDB`. Configure the endpoint to use the persistence through the following configuration API:

snippet: CosmosDBUsage

### Customizing the database or container used

By default, the persister will store records in a database named `NServiceBus` and use a container per endpoint using the endpoint name as to name the container.

Customize the database name using the following configuration API:

snippet: CosmosDBDatabaseName

Customize the container used using the following configuration API

snippet: CosmosDBDefaultContainer

## Transactions

The persister supports using the [Cosmos DB transactional batch API](https://devblogs.microsoft.com/cosmosdb/introducing-transactionalbatch-in-the-net-sdk/). However, Cosmos DB only allows operations to be batched if all operations are performed within the same logical partition key. This is due to the distributed nature of the Cosmos DB service, which [does not support distributed transactions](/nservicebus/azure/understanding-transactionality-in-azure.md).

The [transactions](transactions.md) documentation provides additional details on how to configure NServiceBus to resolve the incoming message to a specific partition key to take advantage of this Cosmos DB feature.

## Outbox cleanup 

When the outbox is enabled, the deduplication data is kept for seven days by default. To customize this time frame, use the following API:

snippet: CosmosDBOutboxCleanup

## Saga concurrency

Due to the distributed nature of Cosmos DB [optimistic concurrency control](https://en.wikipedia.org/wiki/Optimistic_concurrency_control) is always used when updating or deleting saga data. When simultaneously handling messages, conflicts may occur. See below for examples of the exceptions which are thrown. _[Saga concurrency](/nservicebus/sagas/concurrency.md)_ explains how these conflicts are handled, and contains guidance for high-load scenarios.

include: saga-concurrency

### Starting a saga

Example exception:

```
INSERT FULL EXCEPTION TYPE HERE: INSERT MESSAGE HERE
```

### Updating or deleting saga data

Example exception:

```
INSERT FULL EXCEPTION TYPE HERE: INSERT MESSAGE HERE
```
