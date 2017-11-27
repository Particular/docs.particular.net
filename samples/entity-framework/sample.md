---
title: EntityFramework integration
summary: Integrating EntityFramework with NHibernate NServiceBus persistence.
reviewed: 2017-04-19
component: NHibernate
related:
- persistence/nhibernate
---


## Prerequisites

include: sql-prereq

The databases created by this sample are `NsbSamplesEfUowNh` and `NsbSamplesEfUowSql`.


## Running the project

 1. Start the Solution
 1. The text `Press <enter> to send a message` should be displayed in both console windows.
 1. Hit `<enter>` in both console windows.
 
NOTE: In case of exceptions when running the sample, delete tables from the database used by the code. Entity Framework by defult can't update table schemas. If tables use the old schema, the code won't be executed properly.


## Verifying that the sample works correctly

The result in both windows (SQL persistence and NHibernate persistence) should be the same.

 1. The `CreateOrderHandler` displays information that an order was submitted.
 1. The `OrderLifecycleSaga` displays information that the order process has been started.
 1. The `CreateShipmentHandler` displays information that the shipment has been created.
 1. Finally, after a couple of seconds, the `CompleteOrderHandler` displays information that the order is going to be completed.
 
Open SQL Server Management Studio and go to the database. Verify that there is a row in saga state table (`dbo.OrderLifecycleSagaData`), in the orders table (`dbo.Orders`) and in the shipments table (`dbo.Shipments`).


## Code walk-through

This sample contains four projects:

 * Messages - A class library containing the message definitions.
 * Shared - A class library containing the common logic code including data context and handler definitions.
 * Endpoint.NHibernate - A console application running the endpoint with NHibernate persistence.
 * Endpoint.SqlPersitence - A console application running the endpoint with SQL persistence.


### Endpoint projects

The Endpoint mimics a back-end system. It is configured to use SQL Server transport. It uses EntityFramework to store business data (orders and shipments).

When the message arrives at the Receiver, a single transactional data access context is created to ensure consistency of the whole message handling process

 * message is removed from the input queue by the SQL Server transport
 * a new saga instance is created and stored by NHibernate persistence
 * a new `Order` entity is created

snippet: StoreOrder

 * a new `Shipment` entity is created

snippet: StoreShipment

 * a reply message is inserted to the queue
 * a timeout request is inserted to the queue

Notice how storing the shipment retrieves the `Order` from the session cache of Entity Framework. The `Order` is not yet persisted to the database.

### Unit of work

The integration with Entity Framework allows users to take advantage of *Unit of Work* semantics of Entity Framework's `DataContext`. A single instance of the context is shared between all the handlers and the `SaveChanges` method is called after all handlers do their work.

#### Setting up

The setup behavior makes sure that there is an instance of unit of work wrapper class before the handlers are called. 

snippet: SetupBehavior

#### Creating data context

The data context is created only once, before it is first accessed from a handler. In order for the Outbox to work, the business data has to reuse the same connection context as NServiceBus persistence. With NHibernate persistence this is achieved by using the same connection string while within the `TransactionScope`.

snippet: UnitOfWork_NHibernate

With SQL persistence the same goal is achieved by using the same ADO.NET connection and transaction objects in both NServiceBus and Entity Framework.

snippet: UnitOfWork_SQL

The `OnSaveChanges` is used to make sure `SaveChangesAsync` is called when the storage session completes successfully. 
