---
title: Transaction support
summary: The design and implementation details of SQL Server transport transaction support
reviewed: 2019-12-19
component: SqlTransport
redirects:
- nservicebus/sqlserver/transactions
- transports/sqlserver/transactions
---


The SQL Server transport supports the following [transport transaction modes](/transports/transactions.md):

 * Transaction scope (distributed transaction)
 * Transport transaction - Send atomic with receive
 * Transport transaction - receive only
 * Unreliable (transactions disabled)

`TransactionScope` mode is particularly useful as it enables `exactly once` message processing with distributed transactions. However when transport, persistence, and business data are all stored in a single SQL Server catalog, it is possible to achieve `exactly-once` message delivery without distributed transactions. For more details, refer to the [SQL Server native integration](/samples/sqltransport/native-integration/) sample.

NOTE: `Exactly once` message processing without distributed transactions can be achieved with any transport using the [Outbox](/nservicebus/outbox/) feature. It requires business and persistence data to share the storage mechanism but does not put any requirements on transport data storage.


### Transaction scope

partial: ambient-core-warning

In this mode, the ambient transaction is started before receiving the message. The transaction encompasses all stages of processing including user data access and saga data access. 

If either the configured NServiceBus persistence mechanism or the user data access also support transactions via `TransactionScope`, the ambient transaction could be promoted to a distributed transaction.

NOTE: Distributed transactions require Microsoft Distributed Transaction Coordinator (MSDTC) or [Azure SQL Elastic Transactions](https://docs.microsoft.com/en-us/azure/sql-database/sql-database-elastic-transactions-overview).

NOTE: If the persistence mechanisms use SQL Server 2008 or later as an underlying data store and the connection strings configured for the SQL Server transport and the persistence are exactly the same, there will be no DTC escalation as SQL Server is able to handle multiple sequentially opened and closed connections via a local transaction.

include: mssql-dtc-warning

See also a sample covering this mode of operation using either [SQL Persistence](/samples/sqltransport-sqlpersistence/) or [NHibernate Persistence](/samples/sqltransport-nhpersistence/).


### Native transactions

In this mode, the message is received inside a native ADO.NET transaction

partial: native


### Unreliable (transactions disabled)

In this mode, when a message is received it is immediately removed from the input queue. If processing fails the message is lost because the operation cannot be rolled back. Any other operation that is performed when processing the message is executed without a transaction and cannot be rolled back. This can lead to undesired side effects when message processing fails part way through.

partial: custom-connection-and-transaction
