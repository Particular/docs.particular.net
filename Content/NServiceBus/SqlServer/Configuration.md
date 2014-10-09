---
title: The design of SQLServer transport
summary: Design of tables that are used as queues in thr SQLServer transport
tags:
- SQLServer
- Transport
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

Additionally, a clustered index on a ```[RowVersion]``` column is created. The column are directly mapped to the properties of ```NServiceBus.TransportMessage``` class. Receiving messages is conducted via a ```DELETE``` statement from the top of the table (the oldest row according to ```[RowVersion]``` column).

## Concurrency

The SQLServer transport starts up to ```MaximumConcurrencyLevel``` ([set via ```TransportConfig``` section](../msmqtransportconfig.md) threads, each running the receive loop in which it tries to receive a single message via aforementioned ```DELETE``` statement and, if succeeded, passes that message into the processing pipeline. Otherwise it backs up for a while.

## Transactions

The SQLServer transport can work in three modes with regards to transactions. These modes are enabled based on the bus configurations:

### TransactionScope

The ```TransactionScope``` mode is selected by default. It relies or ```Transactions.Enabled``` setting being set to ```true``` and ```Transactions.SuppressDistributedTransactions``` being set to false. One needs to only select the transport:

<!-- import sqlserver-config-transactionscope -->

When in this mode, the receive operation is wrapped in a ```TransactionScope``` together with the message processing in the pipeline. This means that usage of any other persistent resource manager (e.g. RavenDB client, another ```SqlConnection```) will cause escalation of the transaction to full 2-Phase Commit protocol handled via Distributed Transaction Coordinator.

### Native transaction

The native transaction mode requires both ```Transactions.Enabled``` and ```Transactions.SuppressDistributedTransactions``` to be set to ```true```. It can be selcted via

<!-- import sqlserver-config-native-transactions -->

When in this mode, the receive operation is wrapped in a plain ADO.NET ```SqlTransaction```. Both connection and the transaction instances are attached to the pipeline context under these keys ```SqlConnection-{ConnectionString}``` and ```SqlTransaction-{ConnectionString}```. 

### No transaction

The no transaction mode requires ```Transactions.Enabled``` to be set to false which can be achieved via following API call:

<!-- import sqlserver-config-no-transactions -->

When in this mode, the receive operation is not wrapped in any transaction so it is executed in its own implicit transaction by the SQLServer. This means that as soon as the ```DELETE``` operation used for receiving completes, the message is gone and any exception that happens during processing of this message causes it to be permanently lost.

## Primary queue

For each endpoint there is a single primary queue table which name matches the name of the endpoint. This single queue is shared by all instances in case of a scale-out scenario.

## Secondary queues

In order for callbacks 

<!-- import sqlserver-config-callbacks -->

to work in a scale-out scenario each endpoint instance has to have its own queue/table. This is ncessary because callback handlers are stored in-memory in the node that did the send. The reply 

<!-- import sqlserver-config-callbacks-reply -->

should be delivered to this special queue so that it is picked up by the same node that registered the callback.

Secondary queue tables have the name of the machine appended to the name of the primary queue table with ```.``` as separator e.g. ```SomeEndpoint.MyMachine```.

Secondary queues are enabled by default. In order to disable them, one must use the configuration API:

<!-- import sqlserver-config-disable-secondaries -->

Secondary queues use same concurrency model to the primary queue but use different settings for the maximum concurrency level. The default value of this setting is 1 which schould be fine with most scenarios because it is small enough to not degrade the overall performance of the endpoint and large enough to cope with usually small number of callbacks. In order to change this value one should use the configuration API:

<!-- import sqlserver-config-set-secondary-concurrency -->

### Satellites

Secondary queues (and hence callbacks) are disabled for satellite receivers.
