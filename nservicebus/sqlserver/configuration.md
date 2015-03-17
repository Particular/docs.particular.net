---
title: The design of SQL Server transport
summary: Design of tables that are used as queues in thr SQL Server transport
tags:
- SQL Server
- Transports
---

## Queue table structure

Following SQL DDL is used to create a table for a queue:

```SQL
CREATE TABLE [dbo].[{0}](
	[Id] [uniqueidentifier] NOT NULL,
	[CorrelationId] [varchar](255) NULL,
	[ReplyToAddress] [varchar](255) NULL,
	[Recoverable] [bit] NOT NULL,
	[Expires] [datetime] NULL,
	[Headers] [varchar](max) NOT NULL,
	[Body] [varbinary](max) NULL,
	[RowVersion] [bigint] IDENTITY(1,1) NOT NULL
) ON [PRIMARY];
```

Additionally, a clustered index on a `[RowVersion]` column is created. The column are directly mapped to the properties of `NServiceBus.TransportMessage` class. Receiving messages is conducted via a `DELETE` statement from the top of the table (the oldest row according to `[RowVersion]` column).

The tables are created during host install time by [installers](/nservicebus/operations/installers.md). It is required that the user account under which the installation of the host is performed has `CREATE TABLE` as well as `VIEW DEFINITION` permissions on the database in which the queues are to be created. The account under which the service runs does not have to have these permissions. Standard read/write/delete permissions (e.g. being member of `db_datawriter` and `db_datareader` roles) are enough.

## Concurrency

The SQL Server transport adapts the number of receiving threads (up to `MaximumConcurrencyLevel` [set via `TransportConfig` section](/nservicebus/msmq/transportconfig.md)) to the amount of messages waiting for processing. When idle it maintains only one thread that continuously polls the table queue. A more details description of the concurrency control mechanism, as well as the description of behaviour of previous versions, can be found [here](concurrency.md).

## Transactions

The SQL Server transport can work in three modes with regards to transactions. These modes are enabled based on the bus configurations:

### Ambient transaction

The ambient transaction mode is selected by default. It relies or `Transactions.Enabled` setting being set to `true` and `Transactions.SuppressDistributedTransactions` being set to false. One needs to only select the transport:

<!-- import sqlserver-config-transactionscope -->

When in this mode, the receive operation is wrapped in a `TransactionScope` together with the message processing in the pipeline. This means that usage of any other persistent resource manager (e.g. RavenDB client, another `SqlConnection` with different connection string) will cause escalation of the transaction to full two-phase commit protocol handled via Distributed Transaction Coordinator (MS DTC).

### Native transaction

The native transaction mode requires both `Transactions.Enabled` and `Transactions.SuppressDistributedTransactions` to be set to `true`. It can be selcted via

<!-- import sqlserver-config-native-transactions -->

When in this mode, the receive operation is wrapped in a plain ADO.NET `SqlTransaction`. Both connection and the transaction instances are attached to the pipeline context under these keys `SqlConnection-{ConnectionString}` and `SqlTransaction-{ConnectionString}` and are available for user code so that the updates to user data can be done atomically with queue receive operation.

### No transaction

The no transaction mode requires `Transactions.Enabled` to be set to false which can be achieved via following API call:

<!-- import sqlserver-config-no-transactions -->

When in this mode, the receive operation is not wrapped in any transaction so it is executed by the SQL Server in its own implicit transaction.
WARNING: This means that as soon as the `DELETE` operation used for receiving completes, the message is gone and any exception that happens during processing of this message causes it to be permanently lost.

## Primary queue

For each endpoint there is a single primary queue table which name matches the name of the endpoint. This single queue is shared by all instances in case of a scale-out scenario.

## Secondary queues

In order for callbacks e.g.

<!-- import sqlserver-config-callbacks -->

to work in a scale-out scenario each endpoint instance has to have its own queue/table. This is necessary because callback handlers are stored in-memory in the node that did the send. The reply sent via

<!-- import sqlserver-config-callbacks-reply -->

should be delivered to this special queue so that it is picked up by the same node that registered the callback.

Secondary queue tables have the name of the machine appended to the name of the primary queue table with `.` as separator e.g. `SomeEndpoint.MyMachine`.

Secondary queues are enabled by default. In order to disable them, one must use the configuration API:

<!-- import sqlserver-config-disable-secondaries -->

Secondary queues use same adaptive concurrency model to the primary queue. Secondary queues (and hence callbacks) are disabled for satellite receivers.

## Connection strings
   
```xml
<connectionStrings>
   <!-- SQL Server -->
   <add name="NServiceBus/Transport"
        connectionString="Data Source=.\SQLEXPRESS;
                                      Initial Catalog=nservicebus;
                                      Integrated Security=True"/>
</connectionStrings>
```
