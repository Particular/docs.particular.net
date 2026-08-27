---
title: Simple NHibernate Persistence Usage
summary: Using NHibernate to store saga data and business data.
reviewed: 2026-08-27
component: NHibernate
related:
 - nservicebus/sagas
 - persistence
 - persistence/nhibernate
---

## Prerequisites

This sample requires an instance of SQL Server and a database named `Samples.NHibernate`. The database must already exist, as the endpoint creates only the tables it needs.

The `Server` project connects to `localhost,1433` using SQL Server authentication. To run against SQL Server Express instead, use the `.\SqlExpress` connection string shown in the comment directly above the `connectionString` variable in `Program.cs`.

## Code walk-through

This sample shows a simple client/server scenario.

* `Client` sends a `StartOrder` message to `Server`.
* `Server` starts an `OrderSaga`.
* `OrderSaga`:
  * Sends a `ShipOrder` message to itself, and the handler for that message saves `OrderShipped` business data to the database.
  * Requests a timeout with `CompleteOrder` data.
* When the `CompleteOrder` timeout fires, the `OrderSaga` publishes an `OrderCompleted` event.
* `Client` handles the `OrderCompleted` event.

### NHibernate config

NHibernate is configured with the right driver, dialect, and connection string. Then, since NHibernate needs a way to map the class to the database table, the configuration code does this using the `ModelMapper` API. Finally, the configuration is passed to the NServiceBus NHibernate persistence.

snippet: config

### Order saga data

Note that to use NHibernate's lazy-loading feature, all properties on the saga data class must be `virtual`.

snippet: sagadata

### Order saga

snippet: ordersaga

### Handler using ISession

The handler uses the `ISession` instance to store business data.

snippet: handler

## The database

Data in the database is stored in two different tables.

### The saga data

* `IContainSagaData.Id` maps to the `OrderSagaData` primary key and unique identifier column `Id`.
* `IContainSagaData.Originator` and `IContainSagaData.OriginalMessageId` map to columns of the same name with type `varchar(255)`.
* Custom properties on `OrderSagaData`, in this case `OrderDescription` and `OrderId`, are also mapped to columns with the same name and the respective types.

![Query results for the OrderSagaData table](sagadata.png)

### The handler stored data

![Query results for the OrderShipped table](handlerdoc.png)
