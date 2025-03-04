---
title: Simple Azure Table Persistence Usage
summary: Using Azure Table Persistence to store sagas
reviewed: 2025-02-25
component: ASP
related:
 - nservicebus/sagas
redirects:
 - samples/azure/azure-table
---

## Prerequisites

Ensure that an instance of the latest [Azurite Emulator](https://learn.microsoft.com/en-us/azure/storage/common/storage-use-azurite) or [Azure Cosmos DB Emulator](https://docs.microsoft.com/en-us/azure/cosmos-db/local-emulator) is running.

## Projects

### SharedMessages

* The shared message contracts used by all endpoints.

### Client

* Sends the `StartOrder` message to `Server`.
* Receives and handles the `OrderCompleted` event.

### Server

* Receive the `StartOrder` message and initiate an `OrderSaga`.
* `OrderSaga` requests a timeout with an instance of `CompleteOrder` with the saga data.
* `OrderSaga` publishes an `OrderCompleted` event when the `CompleteOrder` timeout fires.

## Persistence config

Configure the endpoint to use Azure Table Persistence.

snippet: AzureTableConfig

In the non-transactional mode the saga id is used as a partition key.

## Order saga data

snippet: sagadata

## Order saga

snippet: thesaga
