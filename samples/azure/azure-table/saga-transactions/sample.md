---
title: Azure Table Persistence Usage using Saga IDs as partition key
summary: Using Azure Table Persistence to store sagas and outbox records atomically using the deterministic Saga ID as the partition key
reviewed: 2020-11-13
component: ASP
related:
 - nservicebus/sagas
---

This sample shows a client/server scenario using saga and outbox persistences to store records atomically by leveraging transactions. The Saga ID is used as a partition key.

## Projects

#### SharedMessages

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

The order id is used to derive the saga id from.

## Behaviors

Most messages implement `IProvideOrderId`. By default Saga IDs are deterministically derived from the saga data, the correlation property name and the correlation property value. `IProvidePartitionKeyFromSagaId` is a helper that can be injected into behaviors in the logical pipeline stage if the Saga ID should be used as a partition key.

snippet: BehaviorUsingIProvidePartitionKeyFromSagaId

One handler replies with a message that doesn't implement `IProvideOrderId`. Transactionality can still be achieved because messages that are part of a saga conversation flow will get the Saga ID set as a header. In such cases no correlation property information needs to be extracted to derive the Saga ID from.

Finally the above behavior is registered in the pipeline.

snippet: BehaviorRegistration

## Order saga data

snippet: sagadata

## Order saga

snippet: thesaga