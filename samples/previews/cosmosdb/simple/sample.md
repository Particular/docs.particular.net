---
title: Simple CosmosDB Persistence Usage
summary: Using CosmosDB Persistence to store sagas
reviewed: 2020-09-22
component: CosmosDB
related:
 - nservicebus/sagas
---

This sample shows a client/server scenario using non-transactional saga persistence.

## Projects

#### SharedMessages

The shared message contracts used by all endpoints.

### Client

 * Sends the `StartOrder` message to `Server`.
 * Receives and handles the `OrderCompleted` event.

### Server projects
 
 * Receive the `StartOrder` message and initiate an `OrderSaga`.
 * `OrderSaga` requests a timeout with an instance of `CompleteOrder` with the saga data.
 * `OrderSaga` publishes an `OrderCompleted` event when the `CompleteOrder` timeout fires.


### Persistence config

Configure the endpoint to use CosmosDB Persistence.

snippet: CosmosDBConfig

In the non-transactional mode the saga id is used as a partition key and thus the container needs to use `/id` as the partition key path.

## Order saga data

snippet: sagadata

## Order saga

snippet: thesaga