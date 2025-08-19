---
title: Cosmos DB Persistence Usage with transactions
summary: Using Cosmos DB Persistence to store sagas and outbox records atomically
reviewed: 2024-10-14
component: CosmosDB
related:
 - nservicebus/sagas
redirects:
 - samples/previews/cosmosdb/transactions
---

This sample shows a client/server scenario using saga and outbox persistences to store records atomically by leveraging transactions.

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

Configure the endpoint to use Cosmos DB Persistence.

snippet: CosmosDBConfig

Because the order id is used as a partition key it has to be used in the partition key path as well.

## Transaction Information

Most messages implement `IProvideOrderId` and thus it is possible to use the provided order identification as a partition key.

snippet: TransactionInformationFromLogicalMessage

One handler publishes an event that doesn't implement `IProvideOrderId` but adds a custom header to indicate the order identification. The handler also creates `OrderShippingInformation` by participating in the transactional batch provided by NServiceBus.

snippet: TransactionInformationFromHeader

## Order saga data

snippet: sagadata

## Order saga

snippet: thesaga
