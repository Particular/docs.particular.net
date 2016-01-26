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

The column are directly mapped to the properties of `NServiceBus.TransportMessage` class. Receiving messages is conducted via a `DELETE` statement from the top of the table (the oldest row according to `[RowVersion]` column).

The tables are created during host install time by [installers](/nservicebus/operations/installers.md). It is required that the user account under which the installation of the host is performed has `CREATE TABLE` as well as `VIEW DEFINITION` permissions on the database in which the queues are to be created. The account under which the service runs does not have to have these permissions. Standard read/write/delete permissions (e.g. being member of `db_datawriter` and `db_datareader` roles) are enough.


## Concurrency

The SQL Server transport adapts the number of receiving threads (up to `MaximumConcurrencyLevel` [set via `TransportConfig` section](/nservicebus/msmq/transportconfig.md)) to the amount of messages waiting for processing. When idle it maintains only one thread that continuously polls the table queue. A more details description of the concurrency control mechanism, as well as the description of behavior of previous versions, can be found [here](concurrency.md).


## Transactions

The SQL Server transport can work in three modes with regards to transactions. These modes are enabled based on the bus configurations:


### Ambient transaction

The ambient transaction mode is selected by default. It relies or `Transactions.Enabled` setting being set to `true` and `Transactions.SuppressDistributedTransactions` being set to false. One needs to only select the transport:

snippet:sqlserver-config-transactionscope

When in this mode, the receive operation is wrapped in a `TransactionScope` together with the message processing in the pipeline. This means that usage of any other persistent resource manager (e.g. RavenDB client, another `SqlConnection` with different connection string) will cause escalation of the transaction to full two-phase commit protocol handled via Distributed Transaction Coordinator (MS DTC).


### Controlling transaction scope options

The following transaction scope options can be configured when the SQL Server transport is working in the ambient transaction mode.


### Isolation level

NServiceBus will by default use the `ReadCommitted` [isolation level](https://msdn.microsoft.com/en-us/library/system.transactions.isolationlevel).

Change the isolation level using

snippet:sqlserver-config-transactionscope-isolation-level


### Transaction timeout

NServiceBus will use the [default transaction timeout](https://msdn.microsoft.com/en-us/library/system.transactions.transactionmanager.defaulttimeout) of the machine the endpoint is running on.

Change the transaction timeout using

snippet:sqlserver-config-transactionscope-timeout

Or via .config file using a [example DefaultSettingsSection](https://msdn.microsoft.com/en-us/library/system.transactions.configuration.defaultsettingssection.aspx#Anchor_5).


### Native transaction

The native transaction mode requires both `Transactions.Enabled` and `Transactions.SuppressDistributedTransactions` to be set to `true`. It can be selected via

snippet:sqlserver-config-native-transactions

When in this mode, the receive operation is wrapped in a plain ADO.NET `SqlTransaction`. Both connection and the transaction instances are attached to the pipeline context under these keys `SqlConnection-{ConnectionString}` and `SqlTransaction-{ConnectionString}` and are available for user code so that the updates to user data can be done atomically with queue receive operation.


### No transaction

The no transaction mode requires `Transactions.Enabled` to be set to false which can be achieved via following API call:

snippet:sqlserver-config-no-transactions

When in this mode, the receive operation is not wrapped in any transaction so it is executed by the SQL Server in its own implicit transaction.

WARNING: This means that as soon as the `DELETE` operation used for receiving completes, the message is gone and any exception that happens during processing of this message causes it to be permanently lost.


## Primary queue

For each endpoint there is a single primary queue table which name matches the name of the endpoint. This single queue is shared by all instances in case of a scale-out scenario.


## Secondary queues

In order for [callbacks](/nservicebus/messaging/handling-responses-on-the-client-side.md) to work in a scale-out scenario each endpoint instance has to have its own queue/table. This is necessary because callback handlers are stored in-memory in the node that did the send. The reply is sent via should be delivered to this special queue so that it is picked up by the same node that registered the callback.

Secondary queue tables have the name of the machine appended to the name of the primary queue table with `.` as separator e.g. `SomeEndpoint.MyMachine`.

Secondary queues are enabled by default. In order to disable them, one must use the configuration API:

snippet:sqlserver-config-disable-secondaries

Secondary queues use same adaptive concurrency model to the primary queue. Secondary queues (and hence callbacks) are disabled for satellite receivers.


## Callback Receiver Max Concurrency

Changes the number of threads that should be used for the callback receiver. The default is 1 thread.

snippet:sqlserver-CallbackReceiverMaxConcurrency


## Circuit Breaker

The Sql transport has a built in circuit breaker to handle intermittent Sql connectivity problems.


### Wait time

Overrides the default time to wait before triggering a circuit breaker that initiates the endpoint shutdown procedure in case there are numerous errors while trying to receive messages.

The default is 2 minutes.

snippet:sqlserver-TimeToWaitBeforeTriggeringCircuitBreaker


### Pause Time

Overrides the default time to pause after a failure while trying to receive a message.

The default is 10 seconds.

snippet: sqlserver-PauseAfterReceiveFailure


## Connection strings

Connection string can be configured in several ways


### Via the App.Config

By adding a connection named `NServiceBus/Transport` in the `connectionStrings` node.
  
```xml
<connectionStrings>
   <!-- SQL Server -->
   <add name="NServiceBus/Transport"
        connectionString="Data Source=.\SQLEXPRESS;
                                      Initial Catalog=nservicebus;
                                      Integrated Security=True"/>
</connectionStrings>
```


### Via the configuration API

By using the `ConnectionString` extension method.

snippet:sqlserver-config-connectionstring


### Via a named connection string

By using the `ConnectionStringName` extension method.

snippet:sqlserver-named-connection-string

Combined with a named connection in the `connectionStrings` node of you `app.config`.

snippet:sqlserver-named-connection-string-xml