---
title: SQL Server Transport and NHibernate Persistence
summary: Integrating SQL Server transport with NHibernate persistence.
reviewed: 2017-06-26
component: Core
related:
- persistence/nhibernate
- transports/sql
---


In this sample, the [SQL Server Transport](/transports/sql/) is used in conjunction with [NHibernate Persistence](/persistence/nhibernate/). The sample shows how to use the same database connection for both transport and persistence operations, and how to access (using multiple [ORMs](https://en.wikipedia.org/wiki/Object-relational_mapping)) the current SQL connection and transaction from within a message handler to persist business objects to the database.

include: persistence-session-note


## Prerequisites

An instance of SQL Server Express is installed and accessible as `.\SqlExpress`.

At startup each endpoint will create its requires SQL assets. For example Receiver will execute the following:

snippet: ReceiverSQLAssets


## Procedure

 1. Start the Sender and Receiver projects.
 1. In the Sender's console notice the `Press <enter> to send a message` text when the app is ready.
 1. Hit enter.
 1. On the Receiver console notice that order was submitted.
 1. On the Sender console notice that the order was accepted.
 1. Finally, after a couple of seconds, on the Receiver console notice that the timeout message has been received.
 1. Open SQL Server Management Studio and go to the `Samples.SqlNHibernate` database. Verify that there is a row in saga state table (`receiver.OrderLifecycleSagaData`) and in the orders table (`receiver.Orders`)


## Code walk-through

This sample contains three projects:

 * Shared - A class library containing common code including the messages definitions.
 * Sender - A console application responsible for sending the initial `OrderSubmitted` message and processing the follow-up `OrderAccepted` message.
 * Receiver - A console application responsible for processing the order message.

Sender and Receiver use different schemas within one database. This creates a separation on logical level (schemas can be secured independently) while retaining the benefits of having a single physical database. Each schema contains, apart from business data, queues for the NServiceBus endpoint and tables for NServiceBus persistence. If no schema is specified, the transport will default to using the `dbo` schema.


### Sender project

The Sender does not store any data. It mimics the front-end system where orders are submitted by the users and passed via the bus to the back-end. It is configured to use SQLServer transport with NHibernate persistence. The transport is configured to use a non-standard schema `sender` and to send messages addressed to `receiver` endpoint to a different schema.

snippet: SenderConfiguration

The connection strings for both persistence and transport need to be exactly the same.


### Receiver project

The Receiver mimics a back-end system. It is also configured to use SQL Server transport with NHibernate persistence but instead of hard-coding the other endpoint's schema, it uses a convention based on the endpoint's queue name.

snippet: ReceiverConfiguration

snippet: NHibernate

When the message arrives at the Receiver, a `TransactionScope` is created that encompasses

 * dequeuing the message
 * persisting business data using the shared session,
 * persisting saga data of `OrderLifecycleSaga` ,
 * sending the reply message and the timeout request.

snippet: Reply

snippet: Timeout

The shared session is managed by NServiceBus hence no need to explicitly begin a transaction or `Flush()` the session.

snippet: StoreUserData

The downside of this approach is, it makes it impossible to use NHibernate's second level cache feature since it requires usage of NHibernate's transactions and letting NHibernate manage its database connections, both of which are disabled when operating in shared connection mode.