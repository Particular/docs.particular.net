---
title: MongoDB Persistence
summary: Learn how to use MongoDB as a persistence mechanism for NServiceBus, including saga and timeout storage.
reviewed: 2026-02-19
component: mongodb
related:
- nservicebus/sagas
redirects:
- samples/mongodb-tekmaven/simple
- samples/mongodb-tekmaven/databus
---

## Prerequisites

Ensure an instance of [MongoDB](https://www.mongodb.com/) is executing on `localhost:27017`.

The easiest way to do this is to run MongoDB in Docker by running the following commands

```shell
docker run -d -p 27017:27017 --name TestMongoDB mongo:latest --replSet tr0
```

```shell
docker exec -it TestMongoDB mongosh --eval '
rs.initiate({
  _id: "tr0",
  members: [{ _id: 0, host: "localhost:27017" }]
})'
```

Alternatively, it is possible to [install MongoDB](https://www.mongodb.com/docs/manual/installation/) but the instance must be part of a [replica set](https://www.mongodb.com/docs/manual/replication/) to enable transactions. If this is not possible, the endpoint configuration must be altered to disable transactions (not recommended for production):

```c#
endpointConfiguration.UsePersistence<MongoPersistence>().UseTransactions(false)
```

### Data visualization

To visualize data in MongoDB, it is useful to install a [MongoDB visualization tool](https://www.mongodb.com/docs/tools-and-connectors/), like for example, [Compass](https://www.mongodb.com/try/download/compass). The screenshots in this sample are taken using Compass.

## Code walk-through

This sample shows a simple client/server scenario:

- `Client` sends a `StartOrder` message to `Server`
- `Server` starts an `OrderSaga` instance
- `OrderSaga` requests a timeout with `CompleteOrder` data
- `CompleteOrder` timeout occurs and `OrderSaga` publishes an `OrderCompleted` event
- `OrderCompleted` is delivered to `Client`, because `Client` is subscribed to that event
- `Client` handles `OrderCompleted`

### MongoDB configuration

The `Server` endpoint is configured to use MongoDB persistence.

snippet: MongoDBConfig

- If a MongoDB URL is not specified, the persistence uses the default of `mongodb://localhost:27017`.
- If a database name is not specified, the persistence uses the endpoint name as the database name. In this sample the database name is `Samples_MongoDB_Server`.

### Order saga

snippet: thesaga

### Saga data

The saga data is stored in the `ordersagadata` collection.

snippet: sagadata

![](sagadata.png)

- `_id` stores `IContainSagaData.Id`
- `Originator` stores `IContainSagaData.Originator`
- `OriginalMessageId` stores `IContainSagaData.OriginalMessageId`
- `OrderID` stores `OrderSagaData.OrderID`
- `OrderDescription` stores `OrderSagaData.OrderDescription`
- `_version` is added and managed by the persistence to prevent concurrency issues
