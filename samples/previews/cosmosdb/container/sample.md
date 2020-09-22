---
title: CosmosDB Persistence Usage with non-default container
summary: Using CosmosDB Persistence to store sagas providing a non-default container dynamically
reviewed: 2020-09-22
component: CosmosDB
related:
 - nservicebus/sagas
---

This sample shows a client/server scenario using a dynamic container configuration for certain saga types with a fallback to the default container.

## Projects

#### SharedMessages

The shared message contracts used by all endpoints.

### Client

 * Sends the `StartOrder` message to `Server`.
 * Receives and handles the `OrderCompleted` event.

### Server projects
 
 * Receive the `StartOrder` message and initiate an `OrderSaga`.
 * `OrderSaga` sends a `ShipOrder` command to `ShipOrderSaga`
 * `ShipOrderSaga` requests a timeout with an instance of `CompleteOrder` with the saga data.
 * `ShipOrderSaga` replies with `CompleteOrder` when the `CompleteOrder` timeout fires.
 * `OrderSaga` publishes an `OrderCompleted` event when the `CompleteOrder` message arrives.


### Persistence config

Configure the endpoint to use CosmosDB Persistence.

snippet: CosmosDBConfig

In the non-transactional mode the saga id is used as a partition key and thus the container needs to use `/id` as the partition key path.

## Behaviors

For all messages destined to go to the `ShipOrderSaga` the container is overridden at runtime to use `ShipOrderSagaData` container.

snippet: BehaviorAddingContainerInfo

The behavior needs to be registered in the pipeline

snippet: BehaviorRegistration

## Order saga data

snippet: ordersagadata

## Order saga

snippet: theordersaga

## ShipOrder saga data

snippet: shipordersagadata

## ShipOrder saga

snippet: theshipordersaga