---
title: Azure Table Persistence Usage with Transactions
summary: Using Azure Table Persistence to store sagas and outbox records atomically
reviewed: 2025-03-06
component: ASP
related:
 - nservicebus/sagas
---

This sample demonstrates a client/server scenario using sagas and outbox persistences to store records atomically by leveraging transactions.

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
* `OrderSaga` publishes an `OrderCompleted` event when the `CompleteOrder` timeout is triggered.

### Persistence config

Configure the endpoint to use Azure Table Persistence.

snippet: AzureTableConfig

The OrderId is used as the partition key.

## Behaviors

Most messages implement `IProvideOrderId` enabling the logical behavior of using the provided OrderId as the partition key.

snippet: BehaviorUsingIProvideOrderId

One of the handlers publishes an event that does not implement `IProvideOrderId` but adds a custom header containing the OrderId. The handler also creates `OrderShippingInformation` as part of the transactional batch provided by NServiceBus.

snippet: UseHeader

The header allows the partition key to be determined within the transport receive context.

snippet: BehaviorUsingHeader

Finally, the above behaviors are registered in the pipeline.

snippet: BehaviorRegistration

## Order saga data

snippet: sagadata

## Order saga

snippet: thesaga
