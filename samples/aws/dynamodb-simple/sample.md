---
title: Simple DynamoDB Persistence Usage
summary: Using DynamoDB Persistence to store sagas
reviewed: 2025-06-04
component: DynamoDB
related:
 - nservicebus/sagas
redirects:
- samples/dynamodb/simple
---

This sample shows a client/server scenario using saga persistence.

## Prerequisites

This sample uses a [DynamoDB local instance](https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/DynamoDBLocal.html) by default. See the [AWS guidance on deploying DynamoDB local](https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/DynamoDBLocal.DownloadingAndRunning.html#docker).

Alternatively with Docker installed locally, execute the following command in the solution directory:

```bash
docker-compose up -d
```

the data is only kept in memory and will be gone when the container is removed.

## Projects

### Client

* Sends the `StartOrder` message to `Server`.
* Receives and handles the `OrderCompleted` event.

### Server projects

* Receive the `StartOrder` message and initiate an `OrderSaga`.
* `OrderSaga` requests a timeout with an instance of `CompleteOrder` with the saga data.
* `OrderSaga` publishes an `OrderCompleted` event when the `CompleteOrder` timeout fires.

### SharedMessages

Contains the shared message contracts used by all endpoints.

## Persistence config

Configure the endpoint to use DynamoDB Persistence.

snippet: DynamoDBConfig

## Order saga data

snippet: sagadata

## Order saga

snippet: thesaga
