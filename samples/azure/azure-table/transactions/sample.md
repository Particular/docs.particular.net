---
title: Azure Table Persistence Usage with Transactions
summary: Using Azure Table Persistence to store sagas and outbox records atomically
reviewed: 2026-10-09
component: ASP
related:
 - nservicebus/sagas
---

This sample demonstrates a client/server scenario using sagas and outbox persistences to store records atomically by leveraging transactions.

## Prerequisites

Ensure that an instance of the latest [Azurite Emulator](https://learn.microsoft.com/en-us/azure/storage/common/storage-use-azurite) or [Azure Cosmos DB Emulator](https://learn.microsoft.com/en-us/azure/cosmos-db/local-emulator) is running.

## Projects

### SharedMessages

The shared message contracts used by all endpoints.

### Client

* Sends the `StartOrder` message to `Server`.
* Receives and handles the `OrderCompleted` event.

### Server

* Receives the `StartOrder` message and initiates an `OrderSaga`.
* `OrderSaga` sends a `ShipOrder` message to itself and requests a timeout with an instance of `CompleteOrder` with the saga data.
* `ShipOrderHandler` handles `ShipOrder`, stores an `OrderShippingInformation` record, and publishes an `OrderShipped` event with a custom header.
* `OrderSaga` receives the `OrderShipped` event.
* `OrderSaga` publishes an `OrderCompleted` event when the `CompleteOrder` timeout is triggered.

## Persistence config

Configure the endpoint to use Azure Table Persistence.

snippet: AzureTableConfig

The saga ID, which is derived from the OrderId, is used as the partition key.

## Using Behaviors

The following shows two different ways to provide OrderIds to the saga using [behaviors](/nservicebus/pipeline/manipulate-with-behaviors.md).

1. Most messages implement `IProvideOrderId` allowing the OrderId to be used as the partition key.

snippet: BehaviorUsingIProvideOrderId

2. One handler publishes an event that does not implement `IProvideOrderId` but adds a custom header containing the OrderId. The handler also creates `OrderShippingInformation` as part of the transactional batch provided by NServiceBus.

snippet: UseHeader

The custom header added then allows the partition key to be determined within `OrderIdHeaderAsPartitionKeyBehavior`, which derives the same saga ID-based partition key from the OrderId in the header.

snippet: BehaviorUsingHeader

Finally, the above behaviors are registered in the pipeline.

snippet: BehaviorRegistration

## Order saga data

snippet: sagadata

## Order saga

snippet: thesaga
