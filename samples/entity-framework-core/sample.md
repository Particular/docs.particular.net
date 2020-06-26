---
title: Entity Framework Core integration with SQL Persistence
summary: Integrating Entity Framework Core with SQL Persistence.
reviewed: 2020-06-26
component: SqlPersistence
related:
- persistence/sql
---


## Prerequisites

include: sql-prereq

The database created by this sample is `NsbSamplesEfCoreUowSql`.


## Running the project

 1. Start the solution
 2. The text `Press <enter> to send a message` will appear in both console windows
 3. Press <kbd>enter</kbd> in both console windows

NOTE: If exceptions occur when running the sample, delete the tables from the database used by the code. By default, Entity Framework will not update table schemas. If tables use an old schema, the code won't execute properly.


## Verifying that the sample works

 1. `CreateOrderHandler` displays information that an order was submitted.
 2. `OrderLifecycleSaga` displays information that the order process has been started.
 3. `CreateShipmentHandler` displays information that the shipment has been created.
 4. After a few seconds, `CompleteOrderHandler` displays information that the order is going to be completed.

Open SQL Server Management Studio and go to the database. Verify that there is a row in the saga state table (`dbo.OrderLifecycleSagaData`), in the orders table (`dbo.Orders`), and in the shipments table (`dbo.Shipments`).


## Code walk-through

This sample contains the following projects:

 * **Messages**: A class library containing the message definitions.
 * **Endpoint.SqlPersistence**: A console application running the endpoint with SQL persistence.


### Endpoint projects

The endpoint mimics a back-end system. It is configured to use the SQL Server transport. It uses Entity Framework to store business data (orders and shipments).

When the message arrives at the receiver, a single transactional data access context is created to ensure consistency of the entire message-handling process:

 * the message is removed from the input queue by the SQL Server transport
 * a new saga instance is created and stored by the SQL persistence
 * a new `Order` entity is created

snippet: StoreOrder

 * a new `Shipment` entity is created

snippet: StoreShipment

 * a reply message is inserted to the queue
 * a timeout request is inserted to the queue

Notice how storing the shipment retrieves the `Order` from the session cache of Entity Framework. The `Order` is not yet persisted to the database.

### Unit of work

The integration with Entity Framework allows users to take advantage of *Unit of Work* semantics of Entity Framework's `DataContext`. A single instance of the context is shared among all handlers and the `SaveChanges` method is called after all handlers do their work.

partial: behavior

#### Creating data context

The data context is created only once, before it is first accessed from a handler. To maintain consistency, the business data has to reuse the same connection context as NServiceBus persistence. With SQL persistence, this is achieved by using the same ADO.NET connection and transaction objects in both NServiceBus and Entity Framework.

snippet: UnitOfWork_SQL

The `OnSaveChanges` event is used to make sure `SaveChangesAsync` is called when the storage session completes successfully.
