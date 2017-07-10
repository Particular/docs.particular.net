---
title: EntityFramework integration
summary: Integrating EntityFramework with NHibernate NServiceBus persistence.
reviewed: 2017-04-19
component: NHibernate
related:
- persistence/nhibernate
---


## Prerequisites

 1. Make sure SQL Server Express is installed and accessible as `.\SqlExpress`.
 1. Create database called `nservicebus`.
 1. In the database create schemas `sender` and `receiver`.


## Running the project

 1. Start the Sender project (right-click on the project, select the `Debug > Start new instance` option).
 1. The text `Press <enter> to send a message` should be displayed in the Sender's console window.
 1. Start the Receiver project (right-click on the project, select the `Debug > Start new instance` option).
 1. In the Sender console hit enter to send a new message.
 
NOTE: In case of exceptions when running the sample, delete tables from the database used by the code (`nservicebus`). Entity Framework by defult can't update table schemas. If tables use the old schema, the code won't be executed properly.


## Verifying that the sample works correctly

 1. The Receiver displays information that an order was submitted.
 1. The Sender displays information that the order was accepted.
 1. Finally, after a couple of seconds, the Receiver displays confirmation that the order has been completed.
 1. Open SQL Server Management Studio and go to the receiver database. Verify that there is a row in saga state table (`dbo.OrderLifecycleSagaData`), in the orders table (`dbo.Orders`) and in the shipments table (`dbo.Shipments`).


## Code walk-through

This sample contains three projects:

 * Shared - A class library containing common code including the message definitions.
 * Sender - A console application responsible for sending the initial `OrderSubmitted` message and processing the follow-up `OrderAccepted` message.
 * Receiver - A console application responsible for processing the order message.

Sender and Receiver use different schemas in the same database. The database is used for storing technical data by NServiceBus infrastructure, i.e. SQL Server transport and NHibernate persistance, and for storing business data.


### Sender project

The Sender does not store any data. It mimics the front-end system where orders are submitted by the users and passed via the bus to the back-end.


### Receiver project

The Receiver mimics a back-end system. It is also configured to use SQLServer transport with NHibernate persistence. It uses EntityFramework to store business data (orders and shipments).

snippet: ReceiverConfiguration

In order for the Outbox to work, the business data has to reuse the same connection string as NServiceBus persistence:

snippet: EntityFramework

When the message arrives at the Receiver, `TransactionScope` is created to ensure consistency of the whole message handling process

 * message is removed from the input queue by the SQL Server transport
 * a new saga instance is created and stored by NHibernate persistence
 * a new `Order` entity is created

snippet: StoreOrder

 * a new `Shipment` entity is created

snippet: StoreShipment

 * a reply message is inserted to the queue
 * a timeout request is inserted to the queue

### Unit of work

The integration with Entity Framework allows users to take advantage of *Unit of Work* semantics of Entity Framework's `DataContext`. A single instance of the context is shared between all the handlers and the `SaveChanges` method is called after all handlers do their work.

#### Setting up

The setup behavior makes sure that there is an instance of unit of work wrapper class before the handlers are called. 

snippet: SetupBehavior

#### Creating data context

The data context is created only once, before it is first accessed from a handler.

snippet: UnitOfWork

The initialization code captures the connection from the NHibernate persistence storage session and registers `SaveChangesAsync` to be called when the storage session completes successfully (`OnSaveChanges`). 
