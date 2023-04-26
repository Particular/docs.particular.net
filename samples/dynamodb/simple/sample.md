---
title: Simple DynamoDB Persistence Usage
summary: Using DynamoDB Persistence to store sagas
reviewed: 2023-04-26
component: DynamoDB
related:
 - nservicebus/sagas
---

This sample shows a client/server scenario using saga persistence.

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
* `OrderSaga` publishes an `OrderCompleted` event when the `CompleteOrder` timeout fires.

### Persistence config

Configure the endpoint to use DynamoDB Persistence.

snippet: DynamoDBConfig

## Order saga data

snippet: sagadata

## Order saga

snippet: thesaga
