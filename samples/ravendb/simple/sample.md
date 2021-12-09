---
title: Simple RavenDB Persistence Usage
summary: Using RavenDB to store Sagas
component: Raven
related:
 - nservicebus/sagas
 - persistence/ravendb
reviewed: 2021-06-12
---

include: dtc-warning

include: cluster-configuration-info


## Code walk-through

This sample shows a simple Client + Server scenario.

 1. `Client` sends a `StartOrder` message to `Server`.
 2. `Server` starts an `OrderSaga`.
 3. `OrderSaga` requests a timeout with a `CompleteOrder` data.
 4. When the `CompleteOrder` timeout fires the `OrderSaga` publishes a `OrderCompleted` event.
 5. The Server then publishes a message that the client subscribes to.
 6. `Client` handles `OrderCompleted` event.


### Raven Config

partial: config-description

snippet: config

include: raven-dispose-warning


### Order Saga Data

snippet: sagadata


### Order Saga

snippet: thesaga


### Handler Using Raven Session

The handler access the same Raven `ISession` via `ISessionProvider`.

snippet: handler


## The Data in RavenDB

The data in RavenDB is stored in three different collections.

WARNING: By default, this sample uses the [Learning Transport](/transports/learning/), which has built-in support for timeouts and subscriptions. To see the data for timeouts and subscriptions, it's necessary to change the sample to a different transport that does not have these native features such as [MSMQ](/transports/msmq/).


### The Saga Data

 * `IContainSagaData.Id` maps to the native RavenDB document `Id`.
 * `IContainSagaData.Originator` and `IContainSagaData.OriginalMessageId` map to simple properties.
 * Custom properties on the SagaData, in this case `OrderDescription` and `OrderId`, are also mapped to simple properties.

```json
{
    "IdentityDocId": "OrderSagaData/OrderId/33e54adb-10fe-ac05-abdd-a656e8f995b3",
    "Data": {
        "$type": "OrderSagaData, Server",
        "OrderId": "683e7b20-527a-475d-847c-79ef6b0f40a1",
        "OrderDescription": "The saga for order 683e7b20-527a-475d-847c-79ef6b0f40a1",
        "Id": "9bf646a0-25b1-4a79-afe3-adf600965476",
        "Originator": "Samples.RavenDB.Client",
        "OriginalMessageId": "06cdd874-de9b-40bb-8b90-adf600965448"
    },
    "@metadata": {
        "@collection": "SagaDataContainers",
        "Raven-Clr-Type": "NServiceBus.Persistence.RavenDB.SagaDataContainer, NServiceBus.RavenDB",
        "NServiceBus-Persistence-RavenDB-SagaDataContainer-SchemaVersion": "1.0.0"
    }
}
```

### The Handler Stored data

```json
{
    "OrderId": "683e7b20-527a-475d-847c-79ef6b0f40a1",
    "ShippingDate": "2021-12-06T09:07:20.1902988Z",
    "@metadata": {
        "@collection": "OrderShippeds",
        "Raven-Clr-Type": "OrderShipped, Server"
    }
}
```
