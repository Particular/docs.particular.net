---
title: Entity Framework integration
summary: Integrating Entity Framework with NHibernate and SQL persistence.
reviewed: 2018-05-29
component: Core
related:
- persistence/nhibernate
- persistence/sql
---


## Prerequisites

include: sql-prereq

The databases created by this sample are `NsbSamplesEfUowNh` and `NsbSamplesEfUowSql`.


## Running the project

 1. Start the solution
 1. The text `Press <enter> to send a message` will appear in both console windows
 1. Press <kbd>enter</kbd> in both console windows

NOTE: In case of exceptions when running the sample, delete the tables from the database used by the code. Entity Framework by default can't update table schemas. If tables use an old schema, the code won't execute properly.


## Verifying that the sample works

The result in both windows (SQL persistence and NHibernate persistence) should be the same.

 1. `CreateOrderHandler` displays information that an order was submitted.
 1. `OrderLifecycleSaga` displays information that the order process has been started.
 1. `CreateShipmentHandler` displays information that the shipment has been created.
 1. Finally, after a couple of seconds, `CompleteOrderHandler` displays information that the order is going to be completed.

Open SQL Server Management Studio and go to the database. Verify that there is a row in the saga state table (`dbo.OrderLifecycleSagaData`), in the orders table (`dbo.Orders`), and in the shipments table (`dbo.Shipments`).


## Code walk-through

This sample contains four projects:

 * **Messages**: A class library containing the message definitions.
 * **Shared**: A class library containing the common logic code including data context and handler definitions.
 * **Endpoint.NHibernate**: A console application running the endpoint with NHibernate persistence.
 * **Endpoint.SqlPersistence**: A console application running the endpoint with SQL persistence.


### Endpoint projects

The endpoint mimics a back-end system. It is configured to use the SQL Server transport. It uses EntityFramework to store business data (orders and shipments).

When the message arrives at the receiver, a single transactional data access context is created to ensure consistency of the whole message handling process.

 * message is removed from the input queue by the SQL Server transport
 * a new saga instance is created and stored by the NHibernate persistence
 * a new `Order` entity is created

snippet: StoreOrder

 * a new `Shipment` entity is created

snippet: StoreShipment

 * a reply message is inserted to the queue
 * a timeout request is inserted to the queue

Notice how storing the shipment retrieves the `Order` from the session cache of Entity Framework. The `Order` is not yet persisted to the database.

### Unit of work

The integration with Entity Framework allows users to take advantage of *Unit of Work* semantics of Entity Framework's `DataContext`. A single instance of the context is shared among all handlers and the `SaveChanges` method is called after all handlers do their work.

#### Setting up

The setup behavior makes sure that there is an instance of the unit of work wrapper class before the handlers are called.

snippet: SetupBehavior

#### Creating data context

The data context is created only once, before it is first accessed from a handler. In order for the outbox to work, the business data has to reuse the same connection context as NServiceBus persistence. With NHibernate persistence, this is achieved by using the same connection string while within the `TransactionScope`.

snippet: UnitOfWork_NHibernate

With SQL persistence, the same goal is achieved by using the same ADO.NET connection and transaction objects in both NServiceBus and Entity Framework.

snippet: UnitOfWork_SQL

The `OnSaveChanges` event is used to make sure `SaveChangesAsync` is called when the storage session completes successfully.
