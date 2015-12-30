---
title: SQL Server Transport
summary: NServiceBus SQL Server
tags:
- SQL Server
redirects:
- nservicebus/sqlserver/configuration
related:
- samples/outbox/sqltransport-nhpersistence
- samples/sqltransport-nhpersistence
- samples/outbox/sqltransport-nhpersistence-ef
---

Provides support for sending messages over [SQL Server](http://www.microsoft.com/en-au/server-cloud/products/sql-server/) tables.


## Queue table structure

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

Up until version 2.x columns are directly mapped to the properties of `NServiceBus.TransportMessage` class. In version 3.0 the `NServiceBus.TransportMessage` class was replaced with `NServiceBus.Transports.IncomingMessage` and `NServiceBus.Transports.OutgoingMessage`, which are used by incoming and outgoing pipeline, respectively. Receiving messages is conducted via a `DELETE` statement from the top of the table (the oldest row according to `[RowVersion]` column).

The tables are created during host install time by [installers](/nservicebus/operations/installers.md). It is required that the user account under which the installation of the host is performed has `CREATE TABLE` as well as `VIEW DEFINITION` permissions on the database in which the queues are to be created. The account under which the service runs does not have to have these permissions. Standard read/write/delete permissions (e.g. being member of `db_datawriter` and `db_datareader` roles) are enough.


## Concurrency

The SQL Server transport adapts the number of receiving threads (up to `MaximumConcurrencyLevel` [set via `TransportConfig` section](/nservicebus/msmq/transportconfig.md)) to the amount of messages waiting for processing. When idle it maintains only one thread that continuously polls the table queue. A more detailed description of the concurrency control mechanism, as well as the description of behavior of previous versions, can be found [here](concurrency.md).

## Transactions

The general information regarding transactions supported by NServiceBus transports is described in detail [here](/nservicebus/messaging/transactions.md). The SQL Server transport supports all levels of available transaction guarantees.


### Transaction scope (Distributed transaction)

The default transaction level support for SQL Server transport is Transaction scope (or Distributed transaction). It doesn't require any setup, distributed transactions are enabled by default when you choose SQL Server transport:

snippet:sqlserver-config-transactionscope

When in this mode, the receive operation is wrapped in a `TransactionScope` together with the message processing in the pipeline. This means that usage of any other persistent resource manager (e.g. RavenDB client, another `SqlConnection` with different connection string) will cause escalation of the transaction to full two-phase commit protocol handled via Distributed Transaction Coordinator (MS DTC).

### Native transaction (Transport transaction) - Receive only


In this mode the receive operation is wrapped in a plain ADO.NET `SqlTransaction`. This mode guarantees that the message is not permanently deleted from the incoming queue until at least one processing attempt (including storing user data and sending out messages) is finished successfully. It can be selected via

snippet:sqlserver-config-native-transactions-receiveOnly

**Note:** This mode wasn't available for SQL Server transport prior to version 3.0. In older versions the only supported guarantee level for native transactions was *Sends atomic with receive*.


### Native transaction (Transport transaction) -  Sends atomic with Receive

In this mode the outgoing operations are wrapped in the same plain ADO.NET `SqlTransaction` as the current receive operation. This prevents messages being sent to downstream endpoints during retries. 

snippet:sqlserver-config-native-transactions-atomicSendsReceive

Both connection and the transaction instances are attached to the pipeline context  and are available for user code so that the updates to user data can be done atomically with queue receive operation. You can access current transaction via current SQL Server transport storage context, by using property injection:

snippet:sqlserver-config-native-transactions-accessTransaction
			
### No transaction


When in this mode, the receive operation is not wrapped in any transaction so it is executed by the SQL Server in its own implicit transaction.

snippet:sqlserver-config-no-transactions

WARNING: This means that as soon as the `DELETE` operation used for receiving completes, the message is gone and any exception that happens during processing of this message causes it to be permanently lost.

## Queues

### Version 2.x and older
Prior to version 3.0 there were two kinds of queues - primary queues and secondary queues. Primary queues support the standard functionality of the SQL Server transport. Secondary queues are mechanism introduced in order to support [callbacks](/nservicebus/messaging/handling-responses-on-the-client-side.md) in a scale-out scenario. 

For each endpoint there is a single primary queue table which name matches the name of the endpoint. This single queue is shared by all instances in case of a scale-out scenario.

In order for [callbacks](/nservicebus/messaging/handling-responses-on-the-client-side.md) to work in a scale-out scenario each endpoint instance has to have its own queue/table. This is necessary because callback handlers are stored in-memory in the node that did the send. The reply is sent via should be delivered to this special queue so that it is picked up by the same node that registered the callback.

Secondary queue tables have the name of the machine appended to the name of the primary queue table with `.` as separator e.g. `SomeEndpoint.MyMachine`.

Secondary queues are enabled by default. In order to disable them, one must use the configuration API:

snippet:sqlserver-config-disable-secondaries

Secondary queues use same adaptive concurrency model to the primary queue. Secondary queues (and hence callbacks) are disabled for satellite receivers.

Callback Receiver Max Concurrency setting changes the number of threads that should be used for the callback receiver. The default is 1 thread.

snippet:sqlserver-CallbackReceiverMaxConcurrency

### Version 3.0 and higher
In version 3.0 there is only one kind of queues. Each endpoint instance has its own dedicated queue table. The name matches the name of the endpoint instance and contains additional suffix to differentiate between endpoint instances. In short, it's a scale-out first, where set up with a single endpoint instance is treated as a specific case of scale-out. 

//TODO: explain how that could be specified and how one should configure this transport to work correctly, do we have defaults? Run test & check

In order to use callbacks in version 3.x, you need to use a dedicated Nuget package `NServiceBus.Callbacks`. The usage is described [here](/nservicebus/messaging/handling-responses-on-the-client-side.md).


## Circuit Breaker

The Sql transport has a built in circuit breaker to handle intermittent SQL Server connectivity problems.


### Wait time

Overrides the default time to wait before triggering a circuit breaker that initiates the endpoint shutdown procedure in case there are numerous errors while trying to receive messages.

The default is 2 minutes.

snippet:sqlserver-TimeToWaitBeforeTriggeringCircuitBreaker


### Pause Time

Overrides the default time to pause after a failure while trying to receive a message.

The default is 10 seconds.

snippet:sqlserver-PauseAfterReceiveFailure
