---
title: Simple NHibernate Persistence Usage
summary: Using NHibernate to store sagas and timeouts.
reviewed: 2020-01-24
component: NHibernate
tags:
 - Saga
 - Timeout
related:
 - nservicebus/sagas
 - persistence
---


## Prerequisites

The sample relies on `.\SqlExpress` and the existence of a database named `Samples.NHibernate` to run properly.


## Code walk-through

This sample shows a simple client/server scenario.

 * `Client` sends a `StartOrder` message to `Server`.
 * `Server` starts an `OrderSaga`.
 * `OrderSaga` requests a timeout with `CompleteOrder` data.
 * When the `CompleteOrder` timeout fires, the `OrderSaga` publishes a `OrderCompleted` event.
 * `Server` then publishes a message that the client has subscribed to.
 * `Client` handles the `OrderCompleted` event.


### NHibernate config

Configure NHibernate with the right driver, dialect, and connection string. Since NHibernate needs a way to map the class to the database table, the configuration code also does that using the ModelMapper API.

snippet: config


### Order saga data

Note that to use NHibernate's lazy-loading, all the properties on the saga data class must be defined as `virtual`.

snippet: sagadata


### Order saga

snippet: ordersaga


### Handler using ISession

The handler access the `ISession` to store business data.

snippet: handler


## The database

### Saga data

 * `IContainSagaData.Id` maps to the OrderSagaData primary-key and unique identifier column `Id`.
 * `IContainSagaData.Originator` and `IContainSagaData.OriginalMessageId` map to columns of the same name of type `varchar(255)`.
 * Custom properties on SagaData, in this case `OrderDescription` and `OrderId`, are also mapped to columns with the same name and the respecting types.

![](sagadata.png)


### Business data stored by the handler

![](handlerdoc.png)
