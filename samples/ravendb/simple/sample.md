---
title: Simple RavenDB Persistence Usage
summary: Using RavenDB to store Sagas and Timeouts.
reviewed: 2016-03-21
component: Raven
tags:
 - Saga
 - Timeout
related:
 - nservicebus/sagas
 - persistence/ravendb
reviewed: 2017-08-21
---

include: dtc-warning


## Code walk-through

This sample shows a simple Client + Server scenario.

 * `Client` sends a `StartOrder` message to `Server`.
 * `Server` starts an `OrderSaga`.
 * `OrderSaga` requests a timeout with a `CompleteOrder` data.
 * When the `CompleteOrder` timeout fires the `OrderSaga` publishes a `OrderCompleted` event.
 * The Server then publishes a message that the client subscribes to.
 * `Client` handles `OrderCompleted` event.


### Raven Config

Configure the endpoint to use RavenDB persistence.

snippet: config

include: raven-dispose-warning


### In Process Raven Host

It is possible to self-host RavenDB so that no running instance of RavenDB server is required to run the sample.

snippet: ravenhost


### Order Saga Data

snippet: sagadata


### Order Saga

snippet: thesaga


### Handler Using Raven Session

The handler access the same Raven `ISession` via `ISessionProvider`.

snippet: handler


## The Data in RavenDB

The data in RavenDB is stored in three different collections.


### The Saga Data

 * `IContainSagaData.Id` maps to the native RavenDB document `Id`.
 * `IContainSagaData.Originator` and `IContainSagaData.OriginalMessageId` map to simple properties pairs.
 * Custom properties on the SagaData, in this case `OrderDescription` and `OrderId`, are also mapped to simple properties.

![](sagadata.png)


### The Timeouts

 * The subscriber is stored in a `Destination` with the nested properties `Queue` and `Machine`.
 * The endpoint that initiated the timeout is stored in the `OwningTimeoutManager` property.
 * The connected saga ID is stored in a `SagaId` property.
 * The serialized data for the message is stored in a `State` property.
 * The scheduled timestamp for the timeout is stored in a `Time` property.
 * Any headers associated with the timeout are stored in an array of key value pairs.

![](timeouts.png)


### The Subscriptions

Note that the message type maps to multiple subscriber endpoints.

 * The Subscription message type and version are stored in the `MessageType` property.
 * The list of subscribers is stored in a array of objects each containing `Queue` and `MachineName` properties.

![](subscriptions.png)


### The Handler Stored data

![](handlerdoc.png)
