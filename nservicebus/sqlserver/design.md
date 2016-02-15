---
title: SQL Server Transport Design
summary: The design and implementation details of SQL Server Transport
tags:
- SQL Server
---

## Queues

### Primary queue

Each endpoint has a single table representing the primary queue. The name of the primary queue matches the name of the endpoint. 

In a scale-out scenario this single queue is shared by all instances.

### Callback queues

Each endpoint has one or more tables representing callback queues. Their names consist of the endpoint name with appropriate suffixes (machine name in version 2, user-specified or default suffixes in version 3).

Since callback handlers are stored in-memory in the node that registers the callback, the same node should process the message representing reply. To guarantee it, in a scale-out scenario each endpoint instance needs a dedicated callback queue.

### Other queues

Each endpoint has also queues required by timeouts (the exact names and number of queues created depends on the version of the transport). 

Error and audit queues are usually shared among multiple endpoints.

### Queue table structure

Following SQL DDL is used to create a table and its index for a queue:

```SQL
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

CREATE CLUSTERED INDEX [Index_RowVersion] ON [schema].[queuename](
	[RowVersion] ASC
) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
```

In version 2 the columns are directly mapped to the properties of `NServiceBus.TransportMessage` class. In version 3 they are mapped to `NServiceBus.Transports.IncomingMessage` in the incoming pipeline and `NServiceBus.Transports.OutgoingMessage` in the outgoing pipeline. Receiving messages is conducted by a `DELETE` statement from the top of the table (the oldest row according to the `[RowVersion]` column).

The tables are created by [installers](/nservicebus/operations/installers.md) when the application is started for the first time. It is required that the user account under which the installation of the host is performed has `CREATE TABLE` as well as `VIEW DEFINITION` permissions on the database in which the queues are to be created. The account under which the service runs does not have to have these permissions. Standard read/write/delete permissions (e.g. being member of `db_datawriter` and `db_datareader` roles) are enough.

## Transactions and delivery guarantees

### Native transactions

Because of the limitations of NHibernate connection management infrastructure, it is not possible to provide *exactly-once* message processing guarantees solely by means of sharing instances of `SqlConnection` and `SqlTransaction` between the transport and NHibernate. For that reason NServiceBus does not allow that configuration and throws an exception during at start-up.

Fortunately the [Outbox](/nservicebus/outbox/) feature can be used to mitigate that problem. In such scenario the messages are stored in the same physical store as saga and user data and dispatched after the whole processing is finished. NHibernate persistence detects the status of Outbox and the presence of SQLServer transport and automatically stops reusing the transport connection and transaction. All the data access is done within the Outbox ambient transaction.

A sample covering this mode of operation is available [here](/samples/outbox/sqltransport-nhpersistence/).


### Transaction scope

In this mode the ambient transaction is started before receiving of the message and encompasses the all stages of processing including user data access and saga data access. If all the logical data stores (transport, user data, saga data) use the same physical store there is no Distributed Transaction Coordinator (DTC) escalation.

snippet:OutboxSqlServerConnectionStrings

A sample covering this mode of operation is available [here](/samples/sqltransport-nhpersistence/).

## Concurrency

The SQL Server transport adapts the number of receiving threads (up to `MaximumConcurrencyLevel` [set via `TransportConfig` section](/nservicebus/msmq/transportconfig.md)) to the amount of messages waiting for processing. When idle it maintains only one thread that continuously polls the table queue.


### Version 3.0

In version 3.0 and higher SQL Server transport maintains a dedicated monitoring thread for each input queue. It is responsible for detecting the number of messages waiting for delivery and creating receive [tasks](https://msdn.microsoft.com/en-us/library/system.threading.tasks.task.aspx) - one for each pending message. 

The maximum number of concurrent tasks will never exceed `MaximumConcurrencyLevel`. The number of tasks does not translate to the number of running threads which is controlled by the TPL scheduling mechanisms.


### Version 2.1

Version 2.1 of SqlTransprot uses an adaptive concurrency model. The transport adapts the number of polling threads based on the rate of messages coming in. The key concept in this new model is the *ramp up controller* which controls the ramping up of new threads and decommissioning of unnecessary threads. It uses the following algorithm:

 * if last receive operation yielded a message, it increments the *consecutive successes* counter and resets the *consecutive failures* counter
 * if last receive operation yielded no message, it increments the *consecutive failures* counter and resets the *consecutive successes* counter
 * if *consecutive successes* counter goes over a certain threshold and there is less polling threads than `MaximumConcurrencyLevel`, it starts a new polling thread and resets the *consecutive successes* counter
 * if *consecutive failures* counter goes over a certain threshold and there is more than one polling thread it kills one of the polling threads


### Version 2.0

In 2.0 release support for callbacks has been added. Callbacks are implemented by each endpoint instance having a unique [secondary queue](./#secondary-queues). The receive for the secondary queue does not use the `MaximumConcurrencyLevel` and defaults to 1 thread. This value can be adjusted via the configuration API.


### Prior to version 2.0

Prior to 2.0 each endpoint running SQLServer transport spins up a fixed number of threads (controlled by `MaximumConcurrencyLevel` property of `TransportConfig` section) both for input and satellite queues. Each thread runs in loop, polling the database for messages awaiting processing.

The disadvantage of this simple model is the fact that satellites (e.g. Second-Level Retries, Timeout Manager) share the same concurrency settings but usually have much lower throughput requirements. If both SLR and TM are enabled, setting `MaximumConcurrencyLevel` to 10 results in 40 threads in total, each polling the database even if there are no messages to be processed.