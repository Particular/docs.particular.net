---
title: Simple NHibernate Persistence Usage
summary: Using NHibernate to store Sagas and Timeouts.
reviewed: 2016-08-29
component: NHibernate
tags:
 - Saga
 - Timeout
related:
 - nservicebus/sagas
 - persistence
---


## Prerequisites

The samples rely on `.\SqlExpress` and need the database `Samples.NHibernate` to run properly.


## Code walk-through

This sample shows a simple Client + Server scenario.

 * `Client` sends a `StartOrder` message to `Server`.
 * `Server` starts an `OrderSaga`.
 * `OrderSaga` requests a timeout with a `CompleteOrder` data.
 * When the `CompleteOrder` timeout fires the `OrderSaga` publishes a `OrderCompleted` event.
 * The Server then publishes a message that the client has subscribed to.
 * `Client` handles `OrderCompleted` event.


### NHibernate Config

Configure NHibernate with the right driver, dialect and connection string. Since NHibernate needs a form of mapping of the class to the database table, the configuration code also does that with ModelMapper API. Finally, the configuration is used to run the endpoint.

snippet: config


### Order Saga Data

Note that to use NHibernate's lazy-loading, all the properties on the Saga data class must be `virtual`.

snippet: sagadata


### Order Saga

snippet: ordersaga


### Handler Using ISession

The handler access the `ISession` to store business data.

snippet: handler


## The Data in the database

The data in the database is stored in three different tables.

### The Saga Data

 * `IContainSagaData.Id` maps to the OrderSagaData primary-key and unique identifier column `Id`.
 * `IContainSagaData.Originator` and `IContainSagaData.OriginalMessageId` map to columns of the same name of type `varchar(255)`.
 * Custom properties on the SagaData, in this case `OrderDescription` and `OrderId`, are also mapped to columns with the same name and of the respecting types.

![](sagadata.png)


### The Timeouts

 * The subscriber is stored in `Destination` column and includes `Queue` and `Machine` information.
 * The endpoint that initiated the timeout is stored in the `Endpoint` column.
 * The connected saga ID is stored in a `SagaId` column.
 * The serialized data for the message is stored in a `State` column.
 * The scheduled timestamp for the timeout is stored in a `Time` column.
 * Any headers associated with the timeout are stored in an array of key value pairs stored in the 'Headers' column.

![](timeouts.png)


### The Subscriptions

Note that the message type maps to multiple subscriber endpoints.

 * The Subscription message type and version are stored in the `MessageType` column.
 * The list of subscribers is stored in a array of objects each containing `Queue` and `MachineName` information.

![](subscriptions.png)


### The Handler Stored data

![](handlerdoc.png)
