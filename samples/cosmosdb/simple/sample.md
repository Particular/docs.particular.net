---
title: Simple Cosmos DB Persistence Usage
summary: Using Cosmos DB Persistence to store sagas
reviewed: 2025-09-26
component: CosmosDB
related:
 - nservicebus/sagas
redirects:
 - samples/previews/cosmosdb/simple
---

This sample shows a client/server scenario using non-transactional saga persistence.

## Prerequisites

Ensure that an instance of the latest [Azure Cosmos DB Emulator](https://docs.microsoft.com/en-us/azure/cosmos-db/local-emulator) is running.

## Sample structure

This sample contains three projects, `SharedMessages`, `Client` and `Server`.

### SharedMessages

The shared message contracts used by all endpoints.

### Client

* Sends the `StartOrder` message to `Server`.
* Receives and handles the `OrderCompleted` event.

### Server

* Receive the `StartOrder` message and initiate an `OrderSaga`.
* `OrderSaga` requests a timeout with an instance of `CompleteOrder` with the saga data.
* `OrderSaga` publishes an `OrderCompleted` event when the `CompleteOrder` timeout fires.

## Implementation highlights

### Persistence config

In Program.cs of the Server project, the endpoint is configured to use Cosmos DB Persistence:

snippet: CosmosDBConfig

In the non-transactional mode, the saga id is used as a partition key, and thus, the container needs to use `/id` as the partition key path.

## Order saga data

The data stored on the saga is defined on the `OrderSagaData.cs` file inside the `Server` project:

snippet: sagadata

## Order saga

The handler for this data is on the `OrderSaga.cs` file on the `Server` project:

snippet: thesaga
