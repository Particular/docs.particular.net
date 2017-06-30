---
title: Transaction support
summary: The design and implementation details of SQL Server Transport transaction support
reviewed: 2016-08-03
component: SqlServer
tags:
- Transactions
- Transport
redirects:
- nservicebus/sqlserver/transactions
- transports/sqlserver/transactions
---


SQL Server transport supports the following [Transport Transaction Modes](/transports/transactions.md):

 * Transaction scope (Distributed transaction)
 * Transport transaction - Sends atomic with Receive
 * Transport transaction - Receive Only
 * Unreliable (Transactions Disabled)

`TransactionScope` mode is particularly useful as it enables `exactly once` message processing with usage of distributed transactions. However, when transport, persistence and business data are all stored in a single SQL Server catalog it is possible to achieve `exactly-once` message delivery without distributed transactions. For more details refer to the [SQL Server native integration](/samples/sqltransport/native-integration/) sample.

NOTE: `Exactly once` message processing without distributed transactions can be achieved with any transport using [Outbox](/nservicebus/outbox/). It requires business and persistence data to share the storage mechanism but does not put any requirements on transport data storage.


### Transaction scope (Distributed transaction)

In this mode the ambient transaction is started before receiving the message. The transaction encompasses all stages of processing including user data access and saga data access. 

If either the configured NServiceBus persistence mechanism or the user data access also support transactions via `TransactionScope`, the ambient transaction is escalated to a distributed one via the Distributed Transaction Coordinator (DTC).

NOTE: If the persistence mechanisms use SQL Server 2008 or later as an underlying data store and the connection string configured for the SQL Server transport and the persistence is the same, there will be no DTC escalation as SQL Server is able to handle multiple non-overlapping connections via a local transaction.

include: mssql-dtc-warning

See also a sample covering this mode of operation using either [SQL Persistence](/samples/sqltransport-sqlpersistence/) or [NHibernate Persistence](/samples/sqltransport-nhpersistence/).


### Native transactions

In this mode the message is received inside a native ADO.NET transaction

partial: native


### Unreliable (Transactions Disabled)

In this mode when a message is received it is immediately removed from the input queue. If processing fails the message is lost because the operation cannot be rolled back. Any other operation that is performed when processing the message is executed without a transaction and cannot be rolled back. This can lead to undesired side effects when message processing fails part way through.
