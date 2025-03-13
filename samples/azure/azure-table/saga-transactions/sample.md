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

## Behaviors

Most messages implement `IProvideOrderId`. By default, Saga IDs are deterministically derived from the saga data, the correlation property name, and the correlation property value. `IProvidePartitionKeyFromSagaId` is a helper that can be injected into behaviors in the logical pipeline stage if the Saga ID is used as a partition key.

snippet: BehaviorUsingIProvidePartitionKeyFromSagaId

One of the handlers replies with a message that does not implement `IProvideOrderId`. However, transactionality is still maintained because messages that are part of a saga conversation flow automatically have the Saga ID set as a header. In these cases, there is no need to extract correlation property information to derive the Saga ID.

Finally the above behavior is registered in the pipeline.

snippet: BehaviorRegistration

## Order saga data

snippet: sagadata

## Order saga

snippet: thesaga
