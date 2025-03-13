---
title: Azure Table Persistence Using Saga IDs as Partition Keys
summary: Using Saga IDs as partition keys in Azure Table Persistence to store sagas and outbox records atomically
reviewed: 2025-03-06
component: ASP
related:
 - nservicebus/sagas
---

This sample demonstrates a client/server scenario using saga and outbox persistences to store records atomically by leveraging transactions. The Saga ID is used as a partition key.

## Projects

### SharedMessages

The shared message contracts used by all endpoints.

### Client

* Sends the `StartOrder` message to `Server`.
* Receives and handles the `OrderCompleted` event.

### Server projects

* Receive the `StartOrder` message and initiate an `OrderSaga`.
* `OrderSaga` requests a timeout with an instance of `CompleteOrder` with the saga data.
* Receive the `OrderShipped` message with a custom header.
* `OrderSaga` publishes an `OrderCompleted` event when the `CompleteOrder` timeout fires.

### Persistence config

Configure the endpoint to use Azure Table Persistence.

snippet: AzureTableConfig

## Using Behaviors

The following shows two different ways to provide OrderIDs to the saga using [behaviors](/nservicebus/pipeline/manipulate-with-behaviors.md).

1. Most messages implement `IProvideOrderId` allowing the OrderId to be used as the partition key.
2. 
Most messages implement `IProvideOrderId`. If the Saga ID is used as a partition key, `IProvidePartitionKeyFromSagaId` can be injected into behaviors in the logical pipeline stage.

snippet: BehaviorUsingIProvidePartitionKeyFromSagaId

One of the handlers replies with a message that does not implement `IProvideOrderId`. However, transactionality is still maintained because messages that are part of a saga conversation flow automatically have the Saga ID set as a header. In these cases, there is no need to extract correlation property information to derive the Saga ID.

Finally the above behavior is registered in the pipeline.

snippet: BehaviorRegistration

## Order saga data

snippet: sagadata

## Order saga

snippet: thesaga
