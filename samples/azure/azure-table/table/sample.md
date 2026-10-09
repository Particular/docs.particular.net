---
title: AzureTable Persistence Usage with non-default table
summary: Using Azure Table Persistence to store sagas providing a non-default table dynamically
reviewed: 2026-10-09
component: ASP
related:
 - nservicebus/sagas
---

This sample shows a client/server scenario using a dynamic table configuration for certain saga types with a fallback to the default table.

## Prerequisites

1. Install Docker.
1. If running Docker on Windows, set Docker to use Linux containers.
1. In the sample directory, execute the following to set up the Azurite Azure Storage emulator instance:

> docker run -p 10000:10000 -p 10001:10001 -p 10002:10002 mcr.microsoft.com/azure-storage/azurite

## Projects

### SharedMessages

* The shared message contracts used by all endpoints.

### Client

* Sends the `StartOrder` message to `Server`.
* Receives and handles the `OrderCompleted` event.

### Server

* Receive the `StartOrder` message and initiate an `OrderSaga`.
* `OrderSaga` sends a `ShipOrder` command to `ShipOrderSaga`
* `ShipOrderSaga` requests a timeout with an instance of `CompleteOrder` with the saga data.
* `ShipOrderSaga` replies with `CompleteOrder` when the `CompleteOrder` timeout fires.
* `OrderSaga` publishes an `OrderCompleted` event when the `CompleteOrder` message arrives.

## Persistence config

Configure the endpoint to use Azure Table Persistence.

snippet: AzureTableConfig

In the non-transactional mode the saga id is used as a partition key.

## Behaviors

For all messages destined to go to the `ShipOrderSaga` the table is overridden at runtime to use the `ShipOrderSagaData` table.

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
