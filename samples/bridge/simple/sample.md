---
title: Simple Bridge Usage
summary: How to use the bridge to connect endpoints on different transports
reviewed: 2022-04-15
component: Bridge
related:
 - nservicebus/bridge
---

This sample shows how to bridge two different transports.

For this sample to work without installing any prerequisites, all endpoints use the LearningTransport. To demonstrate that it can still bridge different transports, the LearningTransport is configured to store messages in a different location.

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

Configure the endpoint to use Cosmos DB Persistence.



In the non-transactional mode the saga id is used as a partition key and thus the container needs to use `/id` as the partition key path.

