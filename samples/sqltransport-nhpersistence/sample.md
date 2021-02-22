---
title: SQL Server Transport and NHibernate Persistence
summary: Integrating SQL Server transport with NHibernate persistence.
reviewed: 2021-02-22
component: Core
related:
- persistence/nhibernate
- transports/sql
---


In this sample, the [SQL Server transport](/transports/sql/) is used in conjunction with the [NHibernate persister](/persistence/nhibernate/). The sample shows how to use the same database connection for both transport and persistence operations, and how to access (using multiple [ORMs](https://en.wikipedia.org/wiki/Object-relational_mapping)) the current SQL connection and transaction from within a message handler to persist business objects to the database.

include: persistence-session-note


## Prerequisites

include: sql-prereq

The database created by this sample is `NsbSamplesSqlNHibernate`.


## Procedure

 1. Start the Sender and Receiver projects.
 1. In the Sender's console, press <kbd>enter</kbd>> to send a message when the app is ready.
 1. On the Receiver console, notice that order was submitted.
 1. On the Sender console, notice that the order was accepted.
 1. After a few seconds, on the Receiver console, notice that the timeout message has been received.
 1. Open SQL Server Management Studio and go to the `Samples.SqlNHibernate` database. Verify that there is a row in the saga state table (`receiver.OrderLifecycleSagaData`) and in the orders table (`receiver.Orders`)


## Code walk-through

This sample contains three projects:

 * Shared - A class library containing common code, including the message definitions.
 * Sender - A console application responsible for sending the initial `OrderSubmitted` message and processing the follow-up `OrderAccepted` message.
 * Receiver - A console application responsible for processing the order message.

Sender and Receiver use different schemas within one database. This creates a logical separation (since schemas can be secured independently) while retaining the benefits of having a single physical database. Apart from business data, each schema contains queues for the NServiceBus endpoint and tables for the NServiceBus persister. If no schema is specified, the transport will default to the `dbo` schema.


### Sender project

The Sender does not store any data. It mimics the front-end system where orders are submitted by customers and passed to the back-end via NServiceBus. It is configured to use the SQL Server transport with NHibernate persistence. The transport is configured to use a non-standard schema `sender` and to send messages addressed to the `receiver` endpoint with a different schema.

snippet: SenderConfiguration

The connection strings for both persistence and transport must be exactly the same.


### Receiver project

The Receiver mimics a back-end system. It is also configured to use the SQL Server transport with NHibernate persistence, but instead of hard-coding the other endpoint's schema, it uses a convention based on the endpoint's queue name.

snippet: ReceiverConfiguration

snippet: NHibernate

When the message arrives at the Receiver, a `TransactionScope` is created that:

 * Dequeues the message.
 * Persists business data using the shared session.
 * Persists saga data for `OrderLifecycleSaga`.
 * Sends the reply message and the timeout request.

snippet: Reply

snippet: Timeout

The shared session is managed by NServiceBus, so there is no need to explicitly begin a transaction or `Flush()` the session.

snippet: StoreUserData

One disadvantage of this approach is that it is impossible to use NHibernate's second-level cache feature since it requires that NHibernate manages the transactions and database connections, both of which are disabled when operating in shared connection mode.