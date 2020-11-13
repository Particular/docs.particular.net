---
title: Azure Table Persistence Usage with transactions
summary: Using Azure Table Persistence to store sagas and outbox records atomically
reviewed: 2020-11-13
component: ASP
related:
 - nservicebus/sagas
---

This sample shows a client/server scenario using saga and outbox persistences to store records atomically by leveraging transactions.

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

The order id is used as a partition key.

## Behaviors

Most messages implement `IProvideOrderId` and thus a logical behavior can use the provided order identification as a partition key.

snippet: BehaviorUsingIProvideOrderId

One handler publishes an event that doesn't implement `IProvideOrderId` but adds a custom header to indicate the order identification. The handler also creates `OrderShippingInformation` by participating in the transactional batch provided by NServiceBus.

snippet: UseHeader

The header can be used to determine the partition key in the transport receive context

snippet: BehaviorUsingHeader

Finally the above behaviors are registered in the pipeline.

snippet: BehaviorRegistration

## Order saga data

snippet: sagadata

## Order saga

snippet: thesaga