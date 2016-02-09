---
title: SQL Server Transport Design
summary: The design and implementation details of SQL Server Transport
tags:
- SQL Server
---

## Queues

### Primary queue

For each endpoint there is a single primary queue table which name matches the name of the endpoint. In scale-out scenario this single queue is shared by all instances.


### Secondary queues

In order for [callbacks](/nservicebus/messaging/handling-responses-on-the-client-side.md) to work in a scale-out scenario each endpoint instance has to have its own queue/table. This is necessary because callback handlers are stored in-memory in the node that did the send. The reply should be delivered to the dedicated queue, so the message can be picked up by the same node that registered the callback.

Secondary queue tables have the name of the machine appended to the name of the primary queue table with `.` as separator e.g. `SomeEndpoint.MyMachine`.

Secondary queues are enabled by default. In order to disable them use the configuration API:

snippet:sqlserver-config-disable-secondaries

Secondary queues use same adaptive concurrency model to the primary queue. Secondary queues (and hence callbacks) are disabled for satellite receivers.

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

The column are directly mapped to the properties of `NServiceBus.TransportMessage` class. Receiving messages is conducted via a `DELETE` statement from the top of the table (the oldest row according to `[RowVersion]` column).

The tables are created by [installers](/nservicebus/operations/installers.md) when the application is started for the first time. It is required that the user account under which the installation of the host is performed has `CREATE TABLE` as well as `VIEW DEFINITION` permissions on the database in which the queues are to be created. The account under which the service runs does not have to have these permissions. Standard read/write/delete permissions (e.g. being member of `db_datawriter` and `db_datareader` roles) are enough.


## Concurrency

The SQL Server transport adapts the number of receiving threads (up to `MaximumConcurrencyLevel` [set via `TransportConfig` section](/nservicebus/msmq/transportconfig.md)) to the amount of messages waiting for processing. When idle it maintains only one thread that continuously polls the table queue.


### 3.0

In version 3.0 and higher SQL Server transport maintains a dedicated monitoring thread for each input queue. It is responsible for detecting the number of messages waiting for delivery and creating receive [tasks](https://msdn.microsoft.com/en-us/library/system.threading.tasks.task.aspx) - one for each pending message. 

The maximum number of concurrent tasks will never exceed `MaximumConcurrencyLevel`. The number of tasks does not translate to the number of running threads which is controlled by TPL scheduling mechanisms.


### 2.1

Version 2.1 of SqlTransprot uses an adaptive concurrency model. The transport adapts the number of polling threads based on the rate of messages coming in. The key concept in this new model is the *ramp up controller* which controls the ramping up of new threads and decommissioning of unnecessary threads. It uses the following algorithm:

 * if last receive operation yielded a message, it increments the *consecutive successes* counter and resets the *consecutive failures* counter
 * if last receive operation yielded no message, it increments the *consecutive failures* counter and resets the *consecutive successes* counter
 * if *consecutive successes* counter goes over a certain threshold and there is less polling threads than `MaximumConcurrencyLevel`, it starts a new polling thread and resets the *consecutive successes* counter
 * if *consecutive failures* counter goes over a certain threshold and there is more than one polling thread it kills one of the polling threads


### 2.0

In 2.0 release support for callbacks has been added. Callbacks are implemented by each endpoint instance having a unique [secondary queue](./#secondary-queues). The receive for the secondary queue does not use the `MaximumConcurrencyLevel` and defaults to 1 thread. This value can be adjusted via the configuration API.


### Prior to 2.0

Prior to 2.0 each endpoint running SQLServer transport spins up a fixed number of threads (controlled by `MaximumConcurrencyLevel` property of `TransportConfig` section) both for input and satellite queues. Each thread runs in loop, polling the database for messages awaiting processing.

The disadvantage of this simple model is the fact that satellites (e.g. Second-Level Retries, Timeout Manager) share the same concurrency settings but usually have much lower throughput requirements. If both SLR and TM are enabled, setting `MaximumConcurrencyLevel` to 10 results in 40 threads in total, each polling the database even if there are no messages to be processed.