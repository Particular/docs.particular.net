---
title: DynamoDB Persistence Usage with transactions
summary: Using DynamoDB Persistence to store sagas and outbox records atomically
reviewed: 2023-04-26
component: DynamoDB
related:
 - nservicebus/sagas
---

This sample shows a client/server scenario using saga and outbox persistences to store records atomically by leveraging transactions.

## Prerequisites

Ensure that an instance of the latest [LocalStack](https://localstack.cloud/) is running.

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

snippet: DynamoDBConfig

The handler also creates `OrderShippingInformation` by participating in the transactional batch provided by NServiceBus.

snippet: DynamoDBStorageSession

## Order saga data

snippet: sagadata

## Order saga

snippet: thesaga
