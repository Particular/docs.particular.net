---
title: Custom audit transport
summary: Using ASQ for auditing while using ASB as the main transport
reviewed: 2025-05-28
component: Core
---

This sample shows how an endpoint can be using one transport, and utlising a different transport for auditing.
In this instance, Azure Service Bus is the main transport, and Azure Storage Queues is used as the transport for audit.

## Prerequisites

Ensure that an instance of the latest [Azure Cosmos DB Emulator](https://docs.microsoft.com/en-us/azure/cosmos-db/local-emulator) is running.

## Projects

### SharedMessages

The shared message contracts used by all endpoints.

### Client

* Sends the `StartOrder` message to `Server`.
* Receives and handles the `OrderCompleted` event.

### Server projects

* Receive the `StartOrder` message and initiate an `OrderSaga`.
* `OrderSaga` requests a timeout with an instance of `CompleteOrder` with the saga data.
* `OrderSaga` publishes an `OrderCompleted` event when the `CompleteOrder` timeout fires.

### Persistence config

Configure the endpoint to use Cosmos DB Persistence.

snippet: CosmosDBConfig

In the non-transactional mode, the saga id is used as a partition key, and thus, the container needs to use `/id` as the partition key path.

## Order saga data

snippet: sagadata

## Order saga

snippet: thesaga
