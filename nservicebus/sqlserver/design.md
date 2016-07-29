---
title: SQL Server Transport Design
summary: The design and implementation details of SQL Server Transport
reviewed: 2016-04-20
tags:
- SQL Server
- Transactions
- Transport
---


## Queues


### Primary queue

Each endpoint has a single table representing the primary queue. The name of the primary queue matches the name of the endpoint.

In a scale-out scenario this single queue is shared by all instances.


### Callback queues

Each endpoint has one or more tables representing callback queues. Their names consist of the endpoint name with appropriate suffixes (machine name in Version 2, user-specified or default suffixes in Version 3).

Since callback handlers are stored in-memory in the node that registers the callback, the same node should process the message representing reply. To guarantee this, in a scale-out scenario, each endpoint instance needs a dedicated callback queue.


### Other queues

Each endpoint also has queues required by timeout (the exact names and number of queues created depends on the version of the transport) and retry mechanisms.

Error and audit queues are usually shared among multiple endpoints.


### Queue table structure

Following SQL DDL is used to create a table and its index for a queue:

```sql
CREATE TABLE [schema].[queuename](
	[Id] [uniqueidentifier] NOT NULL,
	[CorrelationId] [varchar](255) NULL,
	[ReplyToAddress] [varchar](255) NULL,
	[Recoverable] [bit] NOT NULL,
	[Expires] [datetime] NULL,
	[Headers] [varchar](max) NOT NULL,
	[Body] [varbinary](max) NULL,
	[RowVersion] [bigint] IDENTITY(1,1) NOT NULL
) ON [PRIMARY];

CREATE CLUSTERED INDEX [Index_RowVersion] ON [schema].[queuename]
(
	[RowVersion] ASC
)
WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]

CREATE NONCLUSTERED INDEX [Index_Expires] ON [schema].[queuename]
(
	[Expires] ASC
)
INCLUDE
(
	[Id],
	[RowVersion]
)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
```

In Version 2 the columns are directly mapped to the properties of `NServiceBus.TransportMessage` class. In Versions 3 and above they are mapped to `NServiceBus.Transports.IncomingMessage` in the incoming pipeline and `NServiceBus.Transports.OutgoingMessage` in the outgoing pipeline. Receiving messages is conducted by a `DELETE` statement from the top of the table (the oldest row according to the `[RowVersion]` column).

The tables are created by [installers](/nservicebus/operations/installers.md) when the application is started for the first time. It is required that the user account under which the installation of the host is performed has `CREATE TABLE` as well as `VIEW DEFINITION` permissions on the database in which the queues are to be created. The account under which the service runs does not have to have these permissions. Standard read/write/delete permissions (e.g. being member of `db_datawriter` and `db_datareader` roles) are enough.

### Creating table structure in Production

The scripts above, to generate the queues, do not have queue names. This may cause confusion when reviewed by a DBA. The scripts could, alternatively, be generated off the Development or Staging environment, and then directly executed on Production environment by DBAs to replicate that table structure. 

To generate this DDL script, right-click the database and from "Tasks" menu choose "Generate Scripts..." and generate the scripts for relevant tables.

![](generating-ddl.png)

### Indexes

Each queue table has a clustered index on the `[RowVersion]` column in order to speed up receiving messages from the queue table.

Starting from version 2.2.2, each queue table also has an additional non-clustered index on the `[Expires]` column. This index speeds up the purging of expired messages from the queue table. If the SQL Server transport discovers that a required index is missing, it will log an appropriate warning. The following SQL statement can be used to create the missing index:

snippet:sql-2.2.2-ExpiresIndex


## Transactions and delivery guarantees

SQL Server transport supports the following [Transport Transaction Modes](/nservicebus/transports/transactions.md):

 * Transaction scope (Distributed transaction)
 * Transport transaction - Sends atomic with Receive
 * Transport transaction - Receive Only
 * Unreliable (Transactions Disabled)


### Transaction scope (Distributed transaction)

In this mode the ambient transaction is started before receiving the message. The transaction encompasses all stages of processing including user data access and saga data access. If all the logical data stores (transport, user data, saga data) use the same physical store there is no escalation to Distributed Transaction Coordinator (DTC).

See also [Sample covering this mode of operation](/samples/sqltransport-nhpersistence/).


### Native transactions

Because of the limitations of NHibernate connection management infrastructure, it is not possible to provide *exactly-once* message processing guarantees solely by means of sharing instances of `SqlConnection` and `SqlTransaction` between the transport and NHibernate. For that reason NServiceBus does not allow that configuration and throws an exception at start-up.

The [Outbox](/nservicebus/outbox/) feature can be used to mitigate that problem. In such scenario the messages are stored in the same physical store as saga and user data and dispatched after the processing is finished. When NHibernate persistence detects the status of Outbox and the presence of SQLServer transport, it automatically stops reusing the transport connection and transaction. Instead the data access is done within the Outbox ambient transaction.

See also [Sample covering this mode of operation](/samples/outbox/sqltransport-nhpersistence/).


### Versions 3 and above

There are two available options within native transaction level:

 * **ReceiveOnly** - An input message is received using native transaction. The transaction is committed only when message processing succeeds.

NOTE: This transaction is not shared outside of the message receiver. That means there is a possibility of persistent side-effects when processing fails, i.e. *ghost messages* might occur.

 * ** SendsAtomicWithReceive** - This mode is similar to the `ReceiveOnly`, but transaction is shared with sending operations. That means the message receive operation and any send or publish operations are committed atomically.


#### Versions 2 and below

There was no distinction between `ReceiveOnly` and `SendsAtomicWithReceive`. Using native transaction was equivalent to `SendsAtomicWithReceive` mode.


### Unreliable (Transactions Disabled)

In this mode when a message is received it is immediately removed from the input queue. If processing fails the message is lost because the operation cannot be rolled back. Any other operation that is performed when processing the message is executed without a transaction and cannot be rolled back. This can lead to undesired side effects when message processing fails part way through.


## Concurrency

The SQL Server transport adapts the number of receiving threads (up to `MaximumConcurrencyLevel` [set via `TransportConfig` section](/nservicebus/msmq/transportconfig.md)) to the amount of messages waiting for processing. When idle it maintains only one thread that continuously polls the table queue.


### Version 3

In Versions 3 and above SQL Server transport maintains a dedicated monitoring thread for each input queue. It is responsible for detecting the number of messages waiting for delivery and creating receive [Task](https://msdn.microsoft.com/en-us/library/system.threading.tasks.task.aspx)s - one for each pending message.

The maximum number of concurrent tasks will never exceed `MaximumConcurrencyLevel`. The number of tasks does not translate to the number of running threads which is controlled by the TPL scheduling mechanisms.


### Version 2.1

Version 2.1 of SqlTransport uses an adaptive concurrency model. The transport adapts the number of polling threads based on the rate of messages coming in. The key concept in this new model is the *ramp up controller* which controls the ramping up of new threads and decommissioning of unnecessary threads. It uses the following algorithm:

 * if last receive operation yielded a message, it increments the *consecutive successes* counter and resets the *consecutive failures* counter
 * if last receive operation yielded no message, it increments the *consecutive failures* counter and resets the *consecutive successes* counter
 * if *consecutive successes* counter goes over a certain threshold and there is less polling threads than `MaximumConcurrencyLevel`, it starts a new polling thread and resets the *consecutive successes* counter
 * if *consecutive failures* counter goes over a certain threshold and there is more than one polling thread it kills one of the polling threads


### Version 2.0

In 2.0 release support for callbacks has been added. Callbacks are implemented by each endpoint instance having a unique [secondary queue](./#secondary-queues). The receive for the secondary queue does not use the `MaximumConcurrencyLevel` and defaults to 1 thread. This value can be adjusted via the configuration API.


### Prior to version 2.0

Prior to 2.0 each endpoint running SQLServer transport spins up a fixed number of threads (controlled by `MaximumConcurrencyLevel` property of `TransportConfig` section) both for input and satellite queues. Each thread runs in loop, polling the database for messages awaiting processing.

The disadvantage of this simple model is the fact that satellites (e.g. Second-Level Retries, Timeout Manager) share the same concurrency settings but usually have much lower throughput requirements. If both SLR and TM are enabled, setting `MaximumConcurrencyLevel` to 10 results in 40 threads in total, each polling the database even if there are no messages to be processed.