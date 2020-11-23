---
title: AzureTable Persistence Usage with non-default table
summary: Using Azure Table Persistence to store sagas providing a non-default table dynamically
reviewed: 2020-11-13
component: ASP
related:
 - nservicebus/sagas
---

This sample shows a client/server scenario using a dynamic table configuration for certain saga types with a fallback to the default table.

## Prerequisites

Ensure that an instance of the latest [Azure Storage Emulator](https://docs.microsoft.com/en-us/azure/storage/storage-use-emulator) or [Azure Cosmos DB Emulator](https://docs.microsoft.com/en-us/azure/cosmos-db/local-emulator) is running.

## Projects

#### SharedMessages

The shared message contracts used by all endpoints.

### Client

 * Sends the `StartOrder` message to `Server`.
 * Receives and handles the `OrderCompleted` event.

### Server projects
 
 * Receive the `StartOrder` message and initiate an `OrderSaga`.
 * `OrderSaga` sends a `ShipOrder` command to `ShipOrderSaga`
 * `ShipOrderSaga` requests a timeout with an instance of `CompleteOrder` with the saga data.
 * `ShipOrderSaga` replies with `CompleteOrder` when the `CompleteOrder` timeout fires.
 * `OrderSaga` publishes an `OrderCompleted` event when the `CompleteOrder` message arrives.


### Persistence config

Configure the endpoint to use Azure Table Persistence.

snippet: AzureTableConfig

In the non-transactional mode the saga id is used as a partition.

## Behaviors

For all messages destined to go to the `ShipOrderSaga` the table is overridden at runtime to use `ShipOrderSagaData` table.

snippet: BehaviorAddingTableInfo

The behavior needs to be registered in the pipeline

snippet: BehaviorRegistration

## Order saga data

snippet: ordersagadata

## Order saga

snippet: theordersaga

## ShipOrder saga data

snippet: shipordersagadata

## ShipOrder saga

snippet: theshipordersaga