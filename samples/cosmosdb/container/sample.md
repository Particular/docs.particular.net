---
title: Cosmos DB Persistence Usage with non-default container
summary: Using Cosmos DB Persistence to store sagas providing a non-default container dynamically
reviewed: 2024-10-15
component: CosmosDB
related:
 - nservicebus/sagas
redirects:
 - samples/previews/cosmosdb/container
---

This sample shows a client/server scenario using a dynamic container configuration for certain saga types with a fallback to the default container.

## Projects

### SharedMessages

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

Configure the endpoint to use Cosmos DB Persistence.

snippet: CosmosDBConfig

In the non-transactional mode the saga id is used as a partition key and thus the container needs to use `/id` as the partition key path.

## Container Mapping

For `ShipOrder` messages destined to go to the `ShipOrderSaga` the container is overridden at runtime to use `ShipOrderSagaData` container.

snippet: ContainerInformationFromLogicalMessage

For all messages that have a saga type header `ShipOrderSaga` the container is overriden to use `ShipOrderSagaData` container too.

snippet: ContainerInformationFromHeaders

## Order saga data

snippet: ordersagadata

## Order saga

snippet: theordersaga

## ShipOrder saga data

snippet: shipordersagadata

## ShipOrder saga

snippet: theshipordersaga
