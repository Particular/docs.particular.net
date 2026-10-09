---
title: Simple Azure Table Persistence Usage
summary: Using Azure Table Persistence to store sagas
reviewed: 2026-10-09
component: ASP
related:
 - nservicebus/sagas
redirects:
 - samples/azure/azure-table
---

This sample demonstrates a client/server scenario that uses Azure Table Persistence to store sagas.

## Prerequisites

Ensure that an instance of the latest [Azurite Emulator](https://learn.microsoft.com/en-us/azure/storage/common/storage-use-azurite) or [Azure Cosmos DB Emulator](https://learn.microsoft.com/en-us/azure/cosmos-db/local-emulator) is running.

## Projects

### SharedMessages

* The shared message contracts used by all endpoints.

### Client

* Sends the `StartOrder` message to `Server`.
* Receives and handles the `OrderCompleted` event.

### Server

* Receives the `StartOrder` message and initiates an `OrderSaga`.
* `OrderSaga` sends a `ShipOrder` message to itself and requests a timeout with an instance of `CompleteOrder` with the saga data.
* `ShipOrderHandler` handles the `ShipOrder` message.
* `OrderSaga` publishes an `OrderCompleted` event when the `CompleteOrder` timeout fires.

## Persistence config

Configure the endpoint to use Azure Table Persistence.

snippet: AzureTableConfig

In the non-transactional mode the saga ID is used as a partition key.

## Order saga data

snippet: sagadata

## Order saga

snippet: thesaga
